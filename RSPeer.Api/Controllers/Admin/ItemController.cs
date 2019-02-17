using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RSPeer.Api.Controllers.Base;
using RSPeer.Application.Features.Store.Items.Commands;
using RSPeer.Application.Features.Store.Items.Queries;
using RSPeer.Domain.Entities;

namespace RSPeer.Api.Controllers.Admin
{
	public class ItemController : BaseController
	{
		[HttpPost]
		public async Task<ActionResult<int>> Add([FromBody] AddItemCommand command)
		{
			return Ok(await Mediator.Send(command));
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Item>>> All()
		{
			return Ok(await Mediator.Send(new GetItemsQuery()));
		}

		[HttpPut]
		public async Task<ActionResult<int>> Update([FromBody] UpdateItemDetailsCommand command)
		{
			return Ok(await Mediator.Send(command));
		}
	}
}