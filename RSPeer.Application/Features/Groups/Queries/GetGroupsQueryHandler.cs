using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Groups.Queries
{
	public class GetGroupsQueryHandler : IRequestHandler<GetGroupsQuery, IEnumerable<Group>>
	{
		private readonly RsPeerContext _db;

		public GetGroupsQueryHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<IEnumerable<Group>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
		{
			return await _db.Groups.ToListAsync(cancellationToken);
		}
	}
}