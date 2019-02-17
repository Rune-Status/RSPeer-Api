using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoMigration;
using RSPeer.Api.Controllers.Base;

namespace RSPeer.Api.Controllers.Admin
{
	public class MongoMigrationController : BaseController
	{
		private readonly MongoMigrationHandler _handler;

		public MongoMigrationController(MongoMigrationHandler handler)
		{
			_handler = handler;
		}

		[HttpPost]
		public async Task<IActionResult> Start()
		{
			await _handler.Execute();
			return Ok();
		}
	}
}