using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Application.Exceptions;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Store.Items.Commands
{
	public class UpdateItemDetailsCommandHandler : IRequestHandler<UpdateItemDetailsCommand, Unit>
	{
		private readonly RsPeerContext _db;

		public UpdateItemDetailsCommandHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<Unit> Handle(UpdateItemDetailsCommand request, CancellationToken cancellationToken)
		{
			var item = await _db.Items.FirstOrDefaultAsync(w => w.Id == request.ItemId, cancellationToken);
			if (item == null) throw new NotFoundException("Item", request.ItemId);

			if (!string.IsNullOrEmpty(request.Name)) item.Name = request.Name;
			if (!string.IsNullOrEmpty(request.Description)) item.Description = request.Description;
			if (request.Price != default(decimal)) item.Price = request.Price;

			_db.Items.Update(item);
			await _db.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}