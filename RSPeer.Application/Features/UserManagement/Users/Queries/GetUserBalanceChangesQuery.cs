using System.Collections.Generic;
using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.UserManagement.Users.Queries
{
	public class GetUserBalanceChangesQuery : IRequest<IEnumerable<BalanceChange>>
	{
		public int UserId { get; set; }
	}
}