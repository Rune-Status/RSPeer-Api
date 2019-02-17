using System;
using MongoDB.Bson;

namespace MongoMigration.Models
{
	public class MongoScriptAccess
	{
		public ObjectId _id { get; set; }
		public ObjectId? ScriptPurchaseId { get; set; }
		public Guid UserId { get; set; }
		public string ScriptId { get; set; }
		public int Limit { get; set; } = -1;
		public DateTime DateAdded { get; set; }
		public DateTime? ExpirationDate { get; set; }
		public bool Recurring { get; set; }
	}
}