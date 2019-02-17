using System;
using System.Collections.Generic;
using MongoDB.Bson;
using RSPeer.Domain.Entities;

namespace MongoMigration.Models
{
	public class MongoScriptPurchase
	{
		public ObjectId _id { get; set; }
		public string BuyerUserId { get; set; }
		public int BuyerTokensAfter { get; set; }
		public string AuthorUserId { get; set; }
		public int PercentTaken { get; set; }
		public int AuthorTokensAfter { get; set; }
		public int TokensTaken { get; set; }
		public string ScriptId { get; set; }
		public int Instances { get; set; }
		public int Price { get; set; }
		public bool IsRefunded { get; set; }
		public DateTime Date { get; set; }

		public bool IsPaidOut { get; set; }

		public Dictionary<string, UserGroup> BuyerGroups { get; set; }
	}
}