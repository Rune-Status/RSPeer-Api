using MediatR;
using RSPeer.Application.Features.UserManagement.Users.Models;

namespace RSPeer.Application.Features.UserManagement.Users.Commands
{
	public class UserCreateSignInJwtCommand : IRequest<UserSignInResult>
	{
		public UserSignInResultPayload Payload { get; set; }
	}
}