using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Store.Purchase.Models
{
	public class PurchaseItemResult
	{
		public decimal Total { get; set; }
		public string Sku { get; set; }
		public PaymentMethod PaymentMethod { get; set; }
		public OrderStatus Status { get; set; }
		public string Meta { get; set; }

		public bool IsCreator { get; set; }
	}
}