using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RSPeer.Api.Controllers.Base;
using RSPeer.Application.Features.Groups.Commands;
using RSPeer.Application.Features.Groups.Queries;

namespace RSPeer.Api.Controllers
{
	public class GroupController : BaseController
	{
		[HttpPost]
		public async Task<ActionResult<int>> Add([FromBody] AddGroupCommand command)
		{
			return Ok(await Mediator.Send(command));
		}

		[HttpGet]
		public async Task<ActionResult<int>> List()
		{
			return Ok(await Mediator.Send(new GetGroupsQuery()));
		}
	}
}