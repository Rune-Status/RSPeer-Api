using System;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.UserManagement.Users.Models
{
	public class UserSignInResultPayload
	{
		public User User { get; set; }
		public DateTimeOffset SignInDate { get; set; }
	}
}