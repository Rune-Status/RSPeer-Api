using MediatR;
using RSPeer.Application.Features.Store.Paypal.Models;

namespace RSPeer.Application.Features.Store.Paypal.Commands
{
	public class PaypalFinishOrderCommand : IRequest<Unit>
	{
		public PaypalPaymentCallback Callback { get; set; }
	}
}