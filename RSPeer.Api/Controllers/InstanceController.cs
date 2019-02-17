using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RSPeer.Api.Controllers.Base;
using RSPeer.Application.Features.Instance.Commands;
using RSPeer.Application.Features.Instance.Queries;
using RSPeer.Domain.Entities;

namespace RSPeer.Api.Controllers
{
	public class InstanceController : BaseController
	{
		[HttpPost]
		public async Task<IActionResult> SaveClient([FromBody] SaveClientInfoCommand command)
		{
			var user = await GetUser();
			command.Client.UserId = user.Id;
			return Ok(await Mediator.Send(command));
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<RunescapeClient>>> RunningClients()
		{
			var user = await GetUser();
			return Ok(await Mediator.Send(new GetRunningClientsQuery { UserId = user.Id }));
		}

		[HttpGet]
		public async Task<ActionResult<int>> RunningClientsCount()
		{
			var user = await GetUser();
			return Ok(await Mediator.Send(new GetRunningClientsCountQuery { UserId = user.Id }));
		}
	}
}