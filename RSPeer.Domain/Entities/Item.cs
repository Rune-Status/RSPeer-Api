using System.Collections.Generic;

namespace RSPeer.Domain.Entities
{
	public class Item
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }
		public string Sku { get; set; }
		public decimal Price { get; set; }
		public PaymentMethod PaymentMethod { get; set; }

		public IEnumerable<Order> Orders { get; set; }
	}

	public enum PaymentMethod
	{
		Tokens,
		Paypal
	}
}