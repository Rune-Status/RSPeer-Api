using System;

namespace RSPeer.Domain.Entities
{
	public class RunescapeClient
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public DateTimeOffset LastUpdate { get; set; }
		public string Ip { get; set; }
		public string ProxyIp { get; set; }
		public string MachineName { get; set; }
		public string OperatingSystem { get; set; }
		public string ScriptName { get; set; }
		public string Rsn { get; set; }
		public string RunescapeEmail { get; set; }

		public bool IsManuallyClosed { get; set; }

		public Guid Tag { get; set; }
	}
}