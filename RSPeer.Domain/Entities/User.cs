using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace RSPeer.Domain.Entities
{
	public class User
	{
		private List<Group> _groups;
		public int Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public int Balance { get; set; }
		public bool IsEmailVerified { get; set; }
		
		public Guid LegacyId { get; set; }

		[JsonIgnore] public ICollection<UserGroup> UserGroups { get; } = new List<UserGroup>();

		[NotMapped]
		public List<Group> Groups
		{
			get => _groups ?? (_groups = UserGroups.Select(w => w.Group).ToList());
			set => _groups = value;
		}
	}
}