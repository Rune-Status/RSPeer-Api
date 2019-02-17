using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoMigration.Models;
using MongoMigration.Util;
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
		private readonly SpacesService _spacesService;

		public MigrateScripts(MongoContext mongo, RsPeerContext db, IConfiguration configuration)
		{
			_mongo = mongo;
			_db = db;
			_spacesService = new SpacesService(configuration);
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
					var scriptListAdd = new List<ScriptMap>();
					var scriptListUpdate = new List<ScriptMap>();

					using (var transaction = await _db.Database.BeginTransactionAsync())
					{
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
								scriptListUpdate.Add(new ScriptMap
								{
									Script = existingScriptNames[script.Name], 
									OldScriptId = script.Identifier
								});
							}
							else
							{
								scriptListAdd.Add(new ScriptMap
								{
									Script = ConvertToScript(userId, script),
									OldScriptId = script.Identifier
								});
							}
						}
					
						await _db.Scripts.AddRangeAsync(scriptListAdd.Select(w => w.Script));
						_db.Scripts.UpdateRange(scriptListUpdate.Select(w => w.Script));
						await _db.SaveChangesAsync();

						var all = new List<ScriptMap>();
						all.AddRange(scriptListAdd);
						all.AddRange(scriptListUpdate);
						
						await SaveScriptContent(all);
						
						transaction.Commit();
					}
					
				
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

		private async Task<byte[]> GetScriptContent(string identifier)
		{
			var script = await _spacesService.Get($"bot/scripts/{identifier}.jar");
			using (var ms = new MemoryStream())
			{
				script.ResponseStream.CopyTo(ms);
				return ms.ToArray();
			}
		}

		private async Task SaveScriptContent(List<ScriptMap> scripts)
		{
			var scriptIds = scripts.Select(w => w.Script.Id);
			var existing = await _db.ScriptContents.AsQueryable().Where(w => scriptIds.Contains(w.ScriptId)).ToListAsync();
			var existingDict = existing.ToDictionary(w => w.ScriptId, w => w);
			
			foreach (var map in scripts)
			{
				Console.WriteLine("Loading Script Content For: " + map.Script.Name);
				var content = new ScriptContent
				{
					ScriptId = map.Script.Id,
					Content = await GetScriptContent(map.OldScriptId)
				};

				if (existingDict.ContainsKey(map.Script.Id))
				{
					existingDict[map.Script.Id].Content = content.Content;
					_db.ScriptContents.Update(existingDict[map.Script.Id]);
				}
				else
				{
					_db.ScriptContents.Add(content);
				}
			}

			await _db.SaveChangesAsync();
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

		public class ScriptMap
		{
			public Script Script { get; set; }
			public string OldScriptId { get; set; }
		}
	}
}