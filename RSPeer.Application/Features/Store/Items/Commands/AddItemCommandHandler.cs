using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nelibur.ObjectMapper;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Store.Items.Commands
{
	public class AddItemCommandHandler : IRequestHandler<AddItemCommand, int>
	{
		private readonly RsPeerContext _db;

		public AddItemCommandHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<int> Handle(AddItemCommand request, CancellationToken cancellationToken)
		{
			var exists = await _db.Items.Where(w => w.Sku == request.Sku).FirstOrDefaultAsync(cancellationToken);

			if (exists != null && !request.Upsert)
				throw new Exception("Item by Sku: " + request.Sku + " already exists.");

			var item = new Item
			{
				Description = request.Description,
				Name = request.Name,
				Sku = request.Sku,
				PaymentMethod = request.PaymentMethod,
				Price = request.Price
			};

			if (exists != null)
			{
				exists = TinyMapper.Map(item, exists);
				_db.Items.Update(exists);
			}
			else
			{
				await _db.Items.AddAsync(item, cancellationToken);
			}

			await _db.SaveChangesAsync(cancellationToken);
			return item.Id;
		}
	}
}