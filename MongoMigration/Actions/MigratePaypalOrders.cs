using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RSPeer.Application.Features.Store.Paypal.Models;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace MongoMigration.Actions
{
	public class MigratePaypalOrders
	{
		private readonly RsPeerContext _db;
		private readonly MongoContext _mongo;

		public MigratePaypalOrders(RsPeerContext db, MongoContext mongo)
		{
			_db = db;
			_mongo = mongo;
		}

		public async Task Execute()
		{
			var orders = _mongo.Database.GetCollection<PaypalFinishedOrder>("PaypalFinishedOrders");

			using (var cursor = orders.AsQueryable().Where(w => w.UserId != null).ToCursor())
			{
				while (await cursor.MoveNextAsync())
				{
					var documents = cursor.Current;
					await OnBatch(documents);
				}
			}

			await _db.SaveChangesAsync();
		}

		private async Task OnBatch(IEnumerable<PaypalFinishedOrder> orders)
		{
			var list = orders
				.ToList();
			var legacyUserIds = list.Select(w => Guid.Parse(w.UserId)).ToList();
			var paypalIds = list.Select(w => w.Id).ToList();
			var users = await _db.Users.AsQueryable().Where(w => legacyUserIds.Contains(w.LegacyId)).ToListAsync();
			var usersDict = users.ToDictionary(w => w.LegacyId, w => w);

			var tokens = await _db.Items.FirstOrDefaultAsync(w => w.Sku == "tokens");

			var existing = await _db.Orders.AsQueryable().Where(w => paypalIds.Contains(w.PaypalId)).Select(w => w.PaypalId)
				.ToListAsync();
			
			var existingSet = existing.ToHashSet();

			foreach (var order in list)
			{
				if (order.UserId == null)
				{
					await _db.Data.AddAsync(new Data
					{
						Key = "import:orders:orderNoUserId",
						Value = order.Id,
					});
					continue;
				}

				var userId = Guid.Parse(order.UserId);
				if (!usersDict.ContainsKey(userId))
				{
					await _db.Data.AddAsync(new Data
					{
						Key = "import:orders:userNotExist",
						Value = userId.ToString(),
					});
					continue;
				}

				if (existingSet.Contains(order.Id))
				{
					continue;
				}

				var entry = new Order
				{
					PaypalId = order.Id,
					IsRefunded = false,
					ItemId = tokens.Id,
					Total = decimal.Parse(order.Transactions.FirstOrDefault()?.Amount.Total),
					Quantity =
						int.Parse(order.Transactions.FirstOrDefault()?.ItemList.Items.FirstOrDefault()?.Quantity),
					Status = OrderStatus.Completed,
					Timestamp = order.CreateTime,
					UserId = usersDict[userId].Id
				};

				await _db.Orders.AddAsync(entry);
			}
		}
	}
}