using Amazon.CognitoIdentityProvider.Model;
using MediatR;

namespace RSPeer.Infrastructure.Cognito.Users.Commands
{
	public class CognitoSignInCommand : IRequest<AuthenticationResultType>
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}
}