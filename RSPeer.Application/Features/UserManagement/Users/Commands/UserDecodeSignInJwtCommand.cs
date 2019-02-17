using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.UserManagement.Users.Commands
{
	public class UserDecodeSignInJwtCommand : IRequest<User>
	{
		public string Token { get; set; }
	}
}