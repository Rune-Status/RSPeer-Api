using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Infrastructure.Cognito.Users.Queries
{
	public class CognitoListUsersQuery : IRequest
	{
		public Func<IEnumerable<User>, Task> Action { get; set; }
	}
}