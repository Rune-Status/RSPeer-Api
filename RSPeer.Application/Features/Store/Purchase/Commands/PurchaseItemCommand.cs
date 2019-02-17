using MediatR;
using RSPeer.Application.Features.Store.Purchase.Models;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Store.Purchase.Commands
{
	public class PurchaseItemCommand : IRequest<PurchaseItemResult>
	{
		public int Quantity { get; set; }

		public string Sku { get; set; }
		public User User { get; set; }
	}
}