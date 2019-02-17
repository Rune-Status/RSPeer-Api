using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RSPeer.Application.Features.UserManagement.Users.Commands;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.UserManagement.Users.Queries
{
	public class GetUserByTokenQueryHandler : IRequestHandler<GetUserByTokenQuery, User>
	{
		private readonly IMediator _mediator;

		public GetUserByTokenQueryHandler(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<User> Handle(GetUserByTokenQuery request, CancellationToken cancellationToken)
		{
			return await _mediator.Send(new UserDecodeSignInJwtCommand { Token = request.Token }, cancellationToken);
		}
	}
}