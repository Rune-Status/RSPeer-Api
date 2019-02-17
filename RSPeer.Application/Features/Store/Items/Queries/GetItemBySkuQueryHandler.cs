using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Store.Items.Queries
{
	public class GetItemBySkuQueryHandler : IRequestHandler<GetItemBySkuQuery, Item>
	{
		private readonly RsPeerContext _db;

		public GetItemBySkuQueryHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<Item> Handle(GetItemBySkuQuery request, CancellationToken cancellationToken)
		{
			return await _db.Items.FirstOrDefaultAsync(w => w.Sku == request.Sku, cancellationToken);
		}
	}
}