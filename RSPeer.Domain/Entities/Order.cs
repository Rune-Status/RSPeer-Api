using System;

namespace RSPeer.Domain.Entities
{
	public class Order
	{
		public int Id { get; set; }
		public int UserId { get; set; }

		public string PaypalId { get; set; }
		
		public decimal Total { get; set; }

		public int Quantity { get; set; }

		public bool IsRefunded { get; set; }

		public Item Item { get; set; }
		public int ItemId { get; set; }
		public DateTimeOffset Timestamp { get; set; }

		public OrderStatus Status { get; set; }

		public bool Recurring { get; set; }
		
		public string LegacyId { get; set; }
	}

	public enum OrderStatus
	{
		Created,
		Processing,
		Completed
	}
}