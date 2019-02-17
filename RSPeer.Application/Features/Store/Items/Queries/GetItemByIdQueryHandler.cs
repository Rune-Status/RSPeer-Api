using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Store.Items.Queries
{
	public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, Item>
	{
		private readonly RsPeerContext _db;

		public GetItemByIdQueryHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<Item> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
		{
			return await _db.Items.FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);
		}
	}
}