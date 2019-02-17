using System.Collections.Generic;
using Newtonsoft.Json;

namespace RSPeer.Domain.Entities
{
	public class Group
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		[JsonIgnore] public ICollection<UserGroup> UserGroups { get; } = new List<UserGroup>();
	}
}