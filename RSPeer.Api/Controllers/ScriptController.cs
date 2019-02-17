using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RSPeer.Api.Controllers.Base;
using RSPeer.Application.Features.Scripts.Commands.ConfirmScript;
using RSPeer.Application.Features.Scripts.Commands.CreateScript;
using RSPeer.Application.Features.Scripts.Queries.GetScript;
using RSPeer.Domain.Entities;

namespace RSPeer.Api.Controllers
{
	public class ScriptController : BaseController
	{
		[HttpPost]
		public async Task<ActionResult<int>> Create([FromBody] CreateScriptCommand command)
		{
			command.User = await GetUser();
			return Ok(await Mediator.Send(command));
		}

		[HttpPost]
		public async Task<ActionResult<int>> Confirm([FromBody] ConfirmScriptCommand command)
		{
			command.User = await GetUser();
			return Ok(await Mediator.Send(command));
		}

		[HttpGet]
		public async Task<ActionResult<Script>> ById([FromQuery] int id)
		{
			return Ok(await Mediator.Send(new GetScriptByIdQuery { ScriptId = id }));
		}


		[HttpGet]
		public async Task<ActionResult<Script>> Content([FromQuery] int id)
		{
			var file = await Mediator.Send(new GetScriptContentQuery { ScriptId = id });
			return new FileContentResult(file?.Content, "application/java-archive");
		}
	}
}