using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RSPeer.Application.Features.UserManagement.Users.Models;
using RSPeer.Application.Features.UserManagement.Users.Queries;
using RSPeer.Infrastructure.Cognito.Users.Commands;

namespace RSPeer.Application.Features.UserManagement.Users.Commands
{
	public class UserSignInCommandHandler : IRequestHandler<UserSignInCommand, UserSignInResult>
	{
		private readonly IMediator _mediator;

		public UserSignInCommandHandler(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<UserSignInResult> Handle(UserSignInCommand request, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new CognitoSignInCommand
			{
				Email = request.Email.ToLower().Trim(),
				Password = request.Password
			}, cancellationToken);

			if (string.IsNullOrEmpty(result.IdToken)) return null;

			var email = await _mediator.Send(new CognitoDecodeTokenCommand { Token = result.IdToken },
				cancellationToken);

			var user = await _mediator.Send(new GetUserByEmailQuery { Email = email, IncludeGroups = true },
				cancellationToken);

			return await _mediator.Send(new UserCreateSignInJwtCommand
			{
				Payload = new UserSignInResultPayload
				{
					SignInDate = DateTimeOffset.UtcNow,
					User = user
				}
			}, cancellationToken);
		}
	}
}