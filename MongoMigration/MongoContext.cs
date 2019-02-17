using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace MongoMigration
{
	public class MongoContext
	{
		private readonly IMongoClient _client;
		public IMongoDatabase Database { get; }

		public MongoContext(IConfiguration configuration)
		{
			_client = new MongoClient(new MongoClientSettings
			{
				Server = new MongoServerAddress(configuration.GetValue<string>("Migration:Mongo"))
			});
			Database = _client.GetDatabase("RsPeer");
		}
	}
}