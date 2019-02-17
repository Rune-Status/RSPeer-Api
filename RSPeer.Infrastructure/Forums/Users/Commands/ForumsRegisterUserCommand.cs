using MediatR;

namespace RSPeer.Infrastructure.Forums.Users.Commands
{
	public class ForumsRegisterUserCommand : IRequest
	{
		public string Username { get; set; }
		public string Email { get; set; }
	}
}