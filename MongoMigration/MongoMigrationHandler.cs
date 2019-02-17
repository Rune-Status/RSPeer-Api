using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using MongoMigration.Actions;
using RSPeer.Persistence;

namespace MongoMigration
{
	public class MongoMigrationHandler
	{
		private readonly MongoContext _mongo;
		private readonly RsPeerContext _db;
		private readonly IMediator _mediator;
		private readonly IConfiguration _configuration;

		public MongoMigrationHandler(MongoContext mongo, RsPeerContext db, IMediator mediator, IConfiguration configuration)
		{
			_mongo = mongo;
			_db = db;
			_mediator = mediator;
			_configuration = configuration;
		}

		public async Task Execute()
		{
			/*
			var users = new MigrateUsers(_mediator);
			await users.Execute();
			
			var scripts = new MigrateScripts(_mongo, _db, _configuration);
			await scripts.Execute();
			
			var scriptAccess = new MigrateScriptAccess(_mongo, _db);
			await scriptAccess.Execute();
			
			var paypalOrders = new MigratePaypalOrders(_db, _mongo);
			await paypalOrders.Execute();
			*/

			var scriptPurchases = new MigrateScriptPurchases(_mongo, _db);
			await scriptPurchases.Execute();
		}
	}
}