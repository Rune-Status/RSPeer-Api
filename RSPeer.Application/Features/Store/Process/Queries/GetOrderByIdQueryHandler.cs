using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Store.Process.Queries
{
	public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Order>
	{
		private readonly RsPeerContext _db;

		public GetOrderByIdQueryHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
		{
			return await _db.Orders.FirstOrDefaultAsync(w => w.Id == request.OrderId, cancellationToken);
		}
	}
}