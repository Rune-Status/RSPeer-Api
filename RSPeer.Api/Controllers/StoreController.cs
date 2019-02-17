using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RSPeer.Api.Controllers.Base;
using RSPeer.Application.Features.Store.Paypal.Models;
using RSPeer.Application.Features.Store.Purchase.Commands;

namespace RSPeer.Api.Controllers
{
	public class StoreController : BaseController
	{
		[HttpPost]
		public async Task<ActionResult<PaypalCreatedOrder>> Purchase([FromBody] PurchaseItemCommand command)
		{
			command.User = await GetUser();
			return Ok(await Mediator.Send(command));
		}
	}
}