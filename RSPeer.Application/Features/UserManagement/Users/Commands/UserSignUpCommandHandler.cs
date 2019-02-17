using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RSPeer.Domain.Entities;
using RSPeer.Infrastructure.Cognito.Users.Commands;
using RSPeer.Infrastructure.Forums.Users.Commands;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.UserManagement.Users.Commands
{
	public class UserSignUpCommandHandler : IRequestHandler<UserSignUpCommand, int>
	{
		private readonly RsPeerContext _db;
		private readonly IMediator _mediator;

		public UserSignUpCommandHandler(IMediator mediator, RsPeerContext db)
		{
			_mediator = mediator;
			_db = db;
		}

		public async Task<int> Handle(UserSignUpCommand request, CancellationToken cancellationToken)
		{
			request = CleanCommand(request);
			using (var transaction = await _db.Database.BeginTransactionAsync(cancellationToken))
			{
				var user = new User
				{
					Username = request.Username,
					Email = request.Email
				};

				await _db.Users.AddAsync(user, cancellationToken);
				await _db.SaveChangesAsync(cancellationToken);

				var cognitoId = await _mediator.Send(new CognitoSignUpCommand
				{
					Email = request.Email,
					Password = request.Password,
					Username = request.Username
				}, cancellationToken);

				if (string.IsNullOrEmpty(cognitoId)) return -1;

				transaction.Commit();

				await _mediator.Send(new ForumsRegisterUserCommand
				{
					Email = request.Email,
					Username = request.Username
				}, cancellationToken);

				return user.Id;
			}
		}

		private UserSignUpCommand CleanCommand(UserSignUpCommand command)
		{
			command.Email = command.Email.ToLower().Trim();
			command.Username = command.Username.Trim();
			return command;
		}
	}
}