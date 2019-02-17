using System;
using MongoDB.Bson;

namespace MongoMigration.Models
{
	public class MongoBalanceChange
	{
		public ObjectId _id { get; set; }
		public string UserId { get; set; }
		public int OldBalance { get; set; }
		public int NewBalance { get; set; }
		public DateTime Date { get; set; } = DateTime.Now;

		public string AdminUsername { get; set; }
		public string AdminUserId { get; set; }
	}
}