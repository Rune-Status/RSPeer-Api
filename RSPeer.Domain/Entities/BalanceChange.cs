using System;

namespace RSPeer.Domain.Entities
{
	public class BalanceChange
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public DateTimeOffset Timestamp { get; set; }
		public int OldBalance { get; set; }
		public int NewBalance { get; set; }
		public int AdminUserId { get; set; }

		public int OrderId { get; set; }
	}
}