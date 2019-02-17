using System;

namespace RSPeer.Domain.Entities
{
	public class Script
	{
		public int Id { get; set; }

		public int UserId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Version { get; set; }

		public ScriptType Type { get; set; }

		public ScriptStatus Status { get; set; }

		public decimal? Price { get; set; }

		public int? Instances { get; set; }

		public int? MaxUsers { get; set; }

		public ScriptCategory Category { get; set; }

		public string ForumThread { get; set; }

		public DateTimeOffset LastUpdate { get; set; }

		public int TotalUsers { get; set; }

		public ScriptContent ScriptContent { get; set; }
	}
	
	public enum ScriptCategory
	{
		Agility,
		Combat,
		Construction,
		Cooking,
		Crafting,
		Farming,
		Firemaking,
		Fishing,
		Fletching,
		Herblore,
		Hunter,
		Magic,
		Minigames,
		Mining,
		MoneyMaking,
		Other,
		Prayer,
		Runecrafting,
		Smithing,
		Thieving,
		Woodcutting,
		Questing
	}

	public enum ScriptStatus
	{
		Pending,
		Live
	}

	public enum ScriptType
	{
		Free,
		Vip,
		PremiumTrial,
		Premium,
		Private
	}
}