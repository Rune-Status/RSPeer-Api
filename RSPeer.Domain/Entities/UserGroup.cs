using Newtonsoft.Json;

namespace RSPeer.Domain.Entities
{
	public class UserGroup
	{
		public int Id { get; set; }
		public int GroupId { get; set; }
		public int UserId { get; set; }

		[JsonIgnore] public User User { get; set; }

		public Group Group { get; set; }
	}
}