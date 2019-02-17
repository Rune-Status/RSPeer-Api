using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RSPeer.Api.Controllers.Base;
using RSPeer.Application.Features.UserManagement.UserGroups.Commands;

namespace RSPeer.Api.Controllers
{
	public class UserGroupController : BaseController
	{
		[HttpPost]
		public async Task<IActionResult> Update([FromBody] UserUpdateGroupsCommand command)
		{
			return Ok(await Mediator.Send(command));
		}
	}
}