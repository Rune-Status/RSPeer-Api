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
	public class MigrateScriptPurchases
	{
		private readonly MongoContext _mongo;
		private readonly RsPeerContext _db;

		public MigrateScriptPurchases(MongoContext mongo, RsPeerContext db)
		{
			_mongo = mongo;
			_db = db;
		}

		public async Task Execute()
		{
			var purchases = _mongo.Database.GetCollection<MongoScriptPurchase>("ScriptPurchases");
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

		private async Task OnBatch(List<MongoScriptPurchase> purchases)
		{
			var legacyIds = purchases.Select(w => w._id.ToString()).ToList();
			var legacyScriptIds = purchases.Select(w => w.ScriptId).ToList();
			var legacyUserIds = purchases.Select(w => Guid.Parse(w.BuyerUserId)).ToList();
			
			var exists = await _db.Orders.Where(w => legacyIds.Contains(w.LegacyId)).Select(w => w.LegacyId).ToListAsync();
			var existsSet = exists.ToHashSet();

			var scripts = await _db.Scripts.Where(w => legacyScriptIds.Contains(w.LegacyId)).ToListAsync();
			var scriptsDict = scripts.ToDictionary(w => w.LegacyId, w => w);


			var users = await _db.Users.Where(w => legacyUserIds.Contains(w.LegacyId)).ToListAsync();
			var usersDict = users.ToDictionary(w => w.LegacyId, w => w);
			
			purchases = purchases.Where(w => !existsSet.Contains(w._id.ToString())).ToList();
			
			foreach (var purchase in purchases)
			{

				if (!scriptsDict.ContainsKey(purchase.ScriptId))
				{
					await _db.Data.AddAsync(new Data
					{
						Key = "import:scriptPurchases:scriptNotFound",
						Value = purchase.ScriptId
					});
					continue;
				}

				if (!usersDict.ContainsKey(Guid.Parse(purchase.BuyerUserId)))
				{
					await _db.Data.AddAsync(new Data
					{
						Key = "import:scriptPurchases:userNotFound",
						Value = purchase.BuyerUserId
					});
					continue;
				}

				var script = scriptsDict[purchase.ScriptId];

				var order = new Order
				{
					IsRefunded = false,
					ItemId = await CreateOrGetItem(script),
					Quantity = 1,
					Timestamp = purchase.Date,
					Status = OrderStatus.Completed,
					Total = purchase.Price,
					LegacyId = purchase._id.ToString(),
					UserId = usersDict[Guid.Parse(purchase.BuyerUserId)].Id,
				};

				await _db.Orders.AddAsync(order);
			}
		}

		private readonly Dictionary<string, Item> skuCache = new Dictionary<string, Item>();
		
		private async Task<int> CreateOrGetItem(Script script)
		{
			var sku = $"premium-script-{script.Id}";
			if (skuCache.ContainsKey(sku))
			{
				return skuCache[sku].Id;
			}
			var item = await _db.Items.FirstOrDefaultAsync(w => w.Sku == $"premium-script-{script.Id}");
			if (item != null)
			{
				skuCache[sku] = item;
				return item.Id;
			}
			item = new Item
			{
				Description = script.Description,
				Name = script.Name,
				Sku = $"premium-script-{script.Id}",
				PaymentMethod = PaymentMethod.Tokens,
				Price = script.Price.GetValueOrDefault(),
			};
			await _db.Items.AddAsync(item);
			await _db.SaveChangesAsync();
			skuCache[sku] = item;
			return item.Id;
		}
	}
}