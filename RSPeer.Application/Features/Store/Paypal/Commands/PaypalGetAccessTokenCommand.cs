using MediatR;

namespace RSPeer.Application.Features.Store.Paypal.Commands
{
	public class PaypalGetAccessTokenCommand : IRequest<string>
	{
		public string BaseUrl { get; set; }
	}
}