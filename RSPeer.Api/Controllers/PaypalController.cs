using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RSPeer.Api.Controllers.Base;
using RSPeer.Application.Features.Store.Paypal.Commands;
using RSPeer.Application.Features.Store.Paypal.Models;

namespace RSPeer.Api.Controllers
{
	public class PaypalController : BaseController
	{
		[HttpPost]
		public async Task<IActionResult> Hook([FromBody] PaypalPaymentCallback order)
		{
			await Mediator.Send(new PaypalFinishOrderCommand { Callback = order });
			return Ok();
		}
	}
}