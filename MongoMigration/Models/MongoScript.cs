using System;
using MongoDB.Bson;
using RSPeer.Domain.Entities;

namespace MongoMigration.Models
{
	public class MongoScript
	{
		public ObjectId _id { get; set; }
		public string Name { get; set; }
		public string NameLower { get; set; }
		public double Version { get; set; }
		public string AuthorUserId { get; set; }
		public string Author { get; set; }
		public string AuthorLower { get; set; }
		public string Description { get; set; }
		public string DescriptionLower { get; set; }
		public ScriptCategory Category { get; set; }

		public string CategoryFormatted => Category.ToString();

		public ScriptMeta Meta { get; set; }
		public string Identifier { get; set; }
		public string Content { get; set; }
		public DateTime DateAdded { get; set; }
		public ulong FileSize { get; set; }
		public DateTime LastUpdate { get; set; }
		public long TotalUsers { get; set; }
		public string RepoUrl { get; set; }
		public string ForumThread { get; set; }
	}

	public enum ScriptType
	{
		Free,
		Premium,
		PremiumTrial,
		Private
	}

	public class ScriptManifest
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Developer { get; set; }
		public string Description { get; set; }
	}

	public class ScriptMeta
	{
		public int Id { get; set; }
		public int Price { get; set; }
		public int Instances { get; set; }
		public ScriptType Type { get; set; }
		public long? MaxUsers { get; set; }
	}
}