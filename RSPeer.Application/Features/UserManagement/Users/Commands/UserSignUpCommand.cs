using MediatR;

namespace RSPeer.Application.Features.UserManagement.Users.Commands
{
	public class UserSignUpCommand : IRequest<int>
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }

		public string PasswordVerify { get; set; }
	}
}