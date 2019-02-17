using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RSPeer.Api.Controllers.Base;
using RSPeer.Application.Features.UserManagement.Sync;

namespace RSPeer.Api.Controllers
{
	public class SyncController : BaseController
	{
		public async Task<IActionResult> Users()
		{
			return Ok(await Mediator.Send(new SyncUsersLocallyCommand()));
		}
	}
}