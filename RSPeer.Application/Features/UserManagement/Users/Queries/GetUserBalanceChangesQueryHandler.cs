using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.UserManagement.Users.Queries
{
	public class
		GetUserBalanceChangesQueryHandler : IRequestHandler<GetUserBalanceChangesQuery, IEnumerable<BalanceChange>>
	{
		private readonly RsPeerContext _db;

		public GetUserBalanceChangesQueryHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<IEnumerable<BalanceChange>> Handle(GetUserBalanceChangesQuery request,
			CancellationToken cancellationToken)
		{
			return await _db.BalanceChanges.Where(w => w.UserId == request.UserId).ToListAsync(cancellationToken);
		}
	}
}