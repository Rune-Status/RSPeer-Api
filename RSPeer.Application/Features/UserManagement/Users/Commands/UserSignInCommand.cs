using MediatR;
using RSPeer.Application.Features.UserManagement.Users.Models;

namespace RSPeer.Application.Features.UserManagement.Users.Commands
{
	public class UserSignInCommand : IRequest<UserSignInResult>
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}
}