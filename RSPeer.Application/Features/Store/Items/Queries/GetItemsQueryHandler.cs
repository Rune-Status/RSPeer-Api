using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Store.Items.Queries
{
	public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, IEnumerable<Item>>
	{
		private readonly RsPeerContext _db;

		public GetItemsQueryHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<IEnumerable<Item>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
		{
			return await _db.Items.ToListAsync(cancellationToken);
		}
	}
}