using MediatR;

namespace RSPeer.Infrastructure.Cognito.Users.Commands
{
	public class CognitoDecodeTokenCommand : IRequest<string>
	{
		public string Token { get; set; }
	}
}