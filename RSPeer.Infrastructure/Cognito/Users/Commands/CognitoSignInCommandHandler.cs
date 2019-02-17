using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Configuration;
using RSPeer.Infrastructure.Cognito.Users.Base;

namespace RSPeer.Infrastructure.Cognito.Users.Commands
{
	public class CognitoSignInCommandHandler : BaseCognitoHandler<CognitoSignInCommand, AuthenticationResultType>
	{
		public override async Task<AuthenticationResultType> Handle(CognitoSignInCommand request,
			CancellationToken cancellationToken)
		{
			using (var provider = new AmazonCognitoIdentityProviderClient(AwsId, AwsKey, RegionEndpoint.USEast1))
			{
				var response = await provider.InitiateAuthAsync(new InitiateAuthRequest
				{
					AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
					ClientId = UserGroupClientId,
					AuthParameters = new Dictionary<string, string>
					{
						{ "USERNAME", request.Email },
						{ "PASSWORD", request.Password },
						{ "EMAIL", request.Email }
					}
				}, cancellationToken);
				response.AuthenticationResult.AccessToken = response.AuthenticationResult.IdToken;
				return response.AuthenticationResult;
			}
		}

		public CognitoSignInCommandHandler(IConfiguration configuration) : base(configuration)
		{
		}
	}
}