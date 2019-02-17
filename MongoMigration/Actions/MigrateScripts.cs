using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoMigration.Models;
using Nelibur.ObjectMapper;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;
using ScriptType = RSPeer.Domain.Entities.ScriptType;

namespace MongoMigration.Actions
{
	public class MigrateScripts
	{
		private readonly MongoContext _mongo;
		private readonly RsPeerContext _db;

		public MigrateScripts(MongoContext mongo, RsPeerContext db)
		{
			_mongo = mongo;
			_db = db;
		}

		public async Task Execute()
		{
			var scripts = _mongo.Database.GetCollection<MongoScript>("Scripts");

			
			using (var cursor = scripts.AsQueryable().Where(w => true).ToCursor())
			{
				var batch = 0;
				while (await cursor.MoveNextAsync())
				{
					var documents = cursor.Current;
					batch++;

					Console.WriteLine($"Batch: {batch}");

					var enumerable = documents.ToList();
					var authors = enumerable.Select(w => w.Author).Distinct().ToList();
					var scriptNames = enumerable.Select(w => w.Name).Distinct().ToList();

					var existing = await _db.Scripts.AsQueryable().Where(w => scriptNames.Contains(w.Name)).ToListAsync();
					var existingScriptNames = existing.ToDictionary(w => w.Name, w => w);

					var userIds = await GetUserIds(authors);
					var scriptListAdd = new List<Script>();
					var scriptListUpdate = new List<Script>();
				
					foreach (var script in enumerable)
					{
						var author = script.Author;
						if (!userIds.ContainsKey(author))
						{
							continue;
						}
						var userId = userIds[author];

						var newScript = ConvertToScript(userId, script);
						
						if (existingScriptNames.ContainsKey(script.Name))
						{
							TinyMapper.Map(newScript, existingScriptNames[script.Name]);
							scriptListUpdate.Add(existingScriptNames[script.Name]);
						}
						else
						{
							scriptListAdd.Add(ConvertToScript(userId, script));
						}
					}
					
					await _db.Scripts.AddRangeAsync(scriptListAdd);
					_db.Scripts.UpdateRange(scriptListUpdate);
					await _db.SaveChangesAsync();
				}

				Console.WriteLine($"Total Batch: { batch}");
			}
		}

		private async Task<Dictionary<string, int>> GetUserIds(List<string> usernames)
		{
			var users = await _db.Users.Where(w => usernames.Contains(w.Username)).ToListAsync();
			return users.ToDictionary(w => w.Username, w => w.Id);
		}
		
		private Script ConvertToScript(int userId, MongoScript script)
		{
			return new Script
			{
				Name = script.Name,
				UserId = userId,
				Status = ScriptStatus.Live,
				Category = script.Category,
				Description = script.Description,
				ForumThread = script.ForumThread,
				Instances = script.Meta?.Instances == -1 || script.Meta?.Instances == 0 ? null : script.Meta?.Instances,
				LastUpdate = script.LastUpdate,
				Price = script.Meta?.Price,
				Type = ConvertScriptType((int?) script.Meta?.Type),
				Version = (decimal) script.Version
			};
		}

		private BsonValue SafeGet(BsonDocument document, string key)
		{
			if (document == null)
			{
				return null;
			}
			return !document.Contains(key) ? null : document[key];
		}

		private ScriptType ConvertScriptType(int? type)
		{
			switch (type)
			{
				case null:
					return ScriptType.Free;
				case 1:
					return ScriptType.Premium;
				case 0:
					return ScriptType.Free;
			}

			return ScriptType.Free;
		}
	}
}