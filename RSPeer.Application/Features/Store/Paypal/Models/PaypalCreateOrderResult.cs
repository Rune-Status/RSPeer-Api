using System;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Store.Paypal.Models
{
	public class PaypalCreateOrderResult
	{
		public string Url { get; set; }
		public User CreatedBy { get; set; }
		public DateTimeOffset Timestamp { get; set; }
		public decimal Total { get; set; }
		public string PaypalId { get; set; }
	}
}