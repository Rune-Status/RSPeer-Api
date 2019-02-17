using MediatR;

namespace RSPeer.Infrastructure.Cognito.Users.Commands
{
	public class CognitoSignUpCommand : IRequest<string>
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}
}