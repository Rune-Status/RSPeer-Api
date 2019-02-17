using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using MediatR;
using Microsoft.Extensions.Configuration;
using RSPeer.Domain.Entities;
using RSPeer.Infrastructure.Cognito.Users.Base;

namespace RSPeer.Infrastructure.Cognito.Users.Queries
{
	public class CognitoListUsersQueryHandler : BaseCognitoHandler<CognitoListUsersQuery, Unit>
	{
		public override async Task<Unit> Handle(CognitoListUsersQuery request, CancellationToken cancellationToken)
		{
			await ListUsers(request.Action);
			return Unit.Value;
		}

		private async Task ListUsers(Func<IEnumerable<User>, Task> action, string paginationToken = null)
		{
			using (var provider = new AmazonCognitoIdentityProviderClient(AwsId, AwsKey, RegionEndpoint.USEast1))
			{
				var res = await provider.ListUsersAsync(new ListUsersRequest
				{
					UserPoolId = UserPoolId,
					PaginationToken = paginationToken
				});
				if (string.IsNullOrEmpty(res.PaginationToken) || res.Users.Count == 0) return;
				var users = res.Users.Select(t => ParseToUser(t.Attributes));
				await action(users);
				await ListUsers(action, res.PaginationToken);
			}
		}

		public CognitoListUsersQueryHandler(IConfiguration configuration) : base(configuration)
		{
		}
	}
}