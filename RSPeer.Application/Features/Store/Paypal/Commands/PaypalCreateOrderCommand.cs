using MediatR;
using RSPeer.Application.Features.Store.Paypal.Models;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Store.Paypal.Commands
{
	public class PaypalCreateOrderCommand : IRequest<PaypalCreateOrderResult>
	{
		public decimal Total { get; set; }
		public int Quantity { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Sku { get; set; }
		public User User { get; set; }

		public string RedirectUrlSuccess { get; set; }
		public string RedirectUrlCancel { get; set; }
	}
}