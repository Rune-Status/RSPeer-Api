using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoMigration.Models;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace MongoMigration.Actions
{
	public class MigrateScriptAccess
	{
		private readonly MongoContext _mongo;
		private readonly RsPeerContext _db;
		
		public MigrateScriptAccess(MongoContext mongo, RsPeerContext db)
		{
			_mongo = mongo;
			_db = db;
		}

		public async Task Execute()
		{
			var access = _mongo.Database.GetCollection<MongoScriptAccess>("ScriptAccess");
			
			using (var cursor = access.AsQueryable().Where(w => true).ToCursor())
			{
				while (await cursor.MoveNextAsync())
				{
					var documents = cursor.Current;
					await OnBatch(documents);
				}
			}

			await _db.SaveChangesAsync();
		}

		private async Task OnBatch(IEnumerable<MongoScriptAccess> access)
		{
			var list = access.ToList();
			var legacyScriptIds = list.Select(w => w.ScriptId).ToList();
			var ids = list.Select(w => w._id.ToString());
			var exists = await _db.ScriptAccess.AsQueryable().Where(w => ids.Contains(w.LegacyId)).ToListAsync();
			var existsDict = exists.ToDictionary(w => w.LegacyId, w => w);

			var scripts = await _db.Scripts.AsQueryable().Where(w => legacyScriptIds.Contains(w.LegacyId)).ToListAsync();
			
			var map = scripts.ToDictionary(w => w.LegacyId, w => w.Id);
			
			list = list.Where(w => !existsDict.ContainsKey(w._id.ToString())).ToList();

			var legacyUserIds = list.Select(w => w.UserId).ToList();

			var userIds = await _db.Users.AsQueryable().Where(w => legacyUserIds.Contains(w.LegacyId)).ToListAsync();
			var userIdsMap = userIds.ToDictionary(w => w.LegacyId, w => w.Id);
			
			foreach (var a in list)
			{
				var mongoId = a._id.ToString();

				if (!userIdsMap.ContainsKey(a.UserId))
				{
					await _db.Data.AddAsync(new Data
					{
						Key = "import:scriptAccess:userIdNotExist",
						Value = a.UserId.ToString()
					});
					continue;
				}

				if (!map.ContainsKey(a.ScriptId))
				{
					await _db.Data.AddAsync(new Data
					{
						Key = "import:scriptAccess:scriptNotExist",
						Value = a.ScriptId
					});
					continue;
				}
				
				var record = new ScriptAccess
				{
					Expiration = a.ExpirationDate.GetValueOrDefault(DateTimeOffset.UtcNow.DateTime),
					Instances = a.Limit == 0 || a.Limit == -1 ? (int?) null : a.Limit,
					Recurring = a.Recurring,
					ScriptId = map[a.ScriptId],
					LegacyId = mongoId,
					UserId = userIdsMap[a.UserId]
				};

				_db.ScriptAccess.Add(record);
			}
		}
	}
}