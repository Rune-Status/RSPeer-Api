using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider.Model;
using MediatR;
using Microsoft.Extensions.Configuration;
using RSPeer.Domain.Entities;

namespace RSPeer.Infrastructure.Cognito.Users.Base
{
	public abstract class BaseCognitoHandler<T, TU> : IRequestHandler<T, TU> where T : IRequest<TU>
	{
		private readonly IConfiguration _configuration;

		protected BaseCognitoHandler(IConfiguration configuration)
		{
			_configuration = configuration;
			UserPoolId =  _configuration.GetValue<string>("AWS:UserPoolId");
			AwsId =  _configuration.GetValue<string>("AWS:AwsId");
			AwsKey =  _configuration.GetValue<string>("AWS:AwsKey");
			UserGroupClientId =  _configuration.GetValue<string>("AWS:UserGroupClientId");
		}

		protected readonly string UserPoolId;
		protected readonly string AwsId;
		protected readonly string AwsKey;
		protected readonly string UserGroupClientId;

		public abstract Task<TU> Handle(T request, CancellationToken cancellationToken);

		protected User ParseToUser(IEnumerable<AttributeType> attributes)
		{
			var a = attributes.ToDictionary(w => w.Name, w => w.Value);
			var email = a.ContainsKey("email") ? a["email"] : null;
			return new User
			{
				LegacyId = a.ContainsKey("sub") ? Guid.Parse(a["sub"]) : Guid.Empty,
				IsEmailVerified = a.ContainsKey("email_verified") && bool.Parse(a["email_verified"]),
				Email = email,
				Username = a.ContainsKey("preferred_username") ? a["preferred_username"] : email,
				Balance =
					a.ContainsKey("custom:balance") ? int.Parse(a["custom:balance"]) : 0
			};
		}
	}
}