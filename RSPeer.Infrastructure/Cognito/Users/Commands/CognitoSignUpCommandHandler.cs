using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Configuration;
using RSPeer.Infrastructure.Cognito.Users.Base;
using RSPeer.Infrastructure.Exceptions;

namespace RSPeer.Infrastructure.Cognito.Users.Commands
{
	public class CognitoSignUpCommandHandler : BaseCognitoHandler<CognitoSignUpCommand, string>
	{
		public override async Task<string> Handle(CognitoSignUpCommand request, CancellationToken cancellationToken)
		{
			using (var provider = new AmazonCognitoIdentityProviderClient(AwsId, AwsKey, RegionEndpoint.USEast1))
			{
				var response = await provider.SignUpAsync(new SignUpRequest
				{
					Password = request.Password,
					ClientId = UserGroupClientId,
					Username = request.Email,
					UserAttributes = new List<AttributeType>
					{
						new AttributeType
						{
							Name = "email",
							Value = request.Email
						},
						new AttributeType
						{
							Name = "preferred_username",
							Value = request.Username
						}
					}
				}, cancellationToken);

				if (response.HttpStatusCode != HttpStatusCode.OK)
					throw new CognitoException("Failed to register user with email " + request.Email + ".");

				return response.UserSub;
			}
		}

		public CognitoSignUpCommandHandler(IConfiguration configuration) : base(configuration)
		{
		}
	}
}