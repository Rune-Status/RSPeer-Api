using System;

namespace RSPeer.Domain.Entities
{
	public class ScriptAccess
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int OrderId { get; set; }
		public DateTimeOffset Timestamp { get; set; }
		public DateTimeOffset Expiration { get; set; }
		public int? Instances { get; set; }
		public bool Recurring { get; set; }
	}
}