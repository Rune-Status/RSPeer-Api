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
	public class MigrateBalanceChanges
	{
			private readonly MongoContext _mongo;
		private readonly RsPeerContext _db;

		public MigrateBalanceChanges(MongoContext mongo, RsPeerContext db)
		{
			_mongo = mongo;
			_db = db;
		}

		public async Task Execute()
		{
			var purchases = _mongo.Database.GetCollection<MongoBalanceChange>("BalanceChange");
			using (var cursor = purchases.AsQueryable().Where(w => true).ToCursor())
			{
				while (await cursor.MoveNextAsync())
				{
					using (var transaction = await _db.Database.BeginTransactionAsync())
					{
					
						var documents = cursor.Current;
						await OnBatch(documents.ToList());
						await _db.SaveChangesAsync();
						transaction.Commit();
					}
				}
			}
			await _db.SaveChangesAsync();
		}

		private async Task OnBatch(List<MongoBalanceChange> list)
		{
			var legacyIds = list.Select(w => w._id.ToString()).ToList();
			
			var legacyUserIds = list.Select(w => Guid.Parse(w.UserId)).ToList();
			var legacyAdminIds = list.Where(w => w.AdminUserId != null).Select(w => Guid.Parse(w.AdminUserId)).ToList();
			
			var users = await _db.Users.AsQueryable().Where(w => legacyUserIds.Contains(w.LegacyId)).ToListAsync();
			var usersDict = users.ToDictionary(w => w.LegacyId, w => w);

			var admins = await _db.Users.AsQueryable().Where(w => legacyAdminIds.Contains(w.LegacyId)).ToListAsync();
			var adminsDict = admins.ToDictionary(w => w.LegacyId, w => w);
			
			var existing = await _db.BalanceChanges.AsQueryable().Where(w => legacyIds.Contains(w.LegacyId)).Select(w => w.LegacyId)
				.ToListAsync();
			var existingSet = existing.ToHashSet();

			list = list.Where(w => !existingSet.Contains(w._id.ToString())).ToList();
			
			foreach (var change in list)
			{

				if (!usersDict.ContainsKey(Guid.Parse(change.UserId)))
				{
					await _db.Data.AddAsync(new Data
					{
						Key = "import:balanceChanges:userNotExist",
						Value = change.UserId
					});
					continue;
				}
				
				var record = new BalanceChange
				{
					LegacyId = change._id.ToString(),
					OldBalance = change.OldBalance,
					NewBalance = change.NewBalance,
					AdminUserId = change.AdminUserId == null ? (int?) null : adminsDict[Guid.Parse(change.AdminUserId)].Id,
					UserId = usersDict[Guid.Parse(change.UserId)].Id,
					Timestamp = change.Date
				};

				await _db.BalanceChanges.AddAsync(record);
			}

		}

	}
}