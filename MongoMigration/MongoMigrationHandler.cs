using System.Threading.Tasks;
using MediatR;
using MongoMigration.Actions;
using RSPeer.Persistence;

namespace MongoMigration
{
	public class MongoMigrationHandler
	{
		private readonly MongoContext _mongo;
		private readonly RsPeerContext _db;
		private readonly IMediator _mediator;

		public MongoMigrationHandler(MongoContext mongo, RsPeerContext db, IMediator mediator)
		{
			_mongo = mongo;
			_db = db;
			_mediator = mediator;
		}

		public async Task Execute()
		{
			var users = new MigrateUsers(_mediator);
			await users.Execute();
			var scripts = new MigrateScripts(_mongo, _db);
			await scripts.Execute();
		}
	}
}