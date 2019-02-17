using System.Collections.Generic;
using Newtonsoft.Json;

namespace RSPeer.Infrastructure.Cognito.Users.Models
{
	public class UserFromJwt
	{
		[JsonProperty("sub")] public string UserId { get; set; }

		[JsonProperty("aud")] public string Aud { get; set; }

		[JsonProperty("cognito:groups")] public List<string> Groups { get; set; }

		[JsonProperty("email_verified")] public bool IsEmailVerified { get; set; }

		[JsonProperty("event_id")] public string EventId { get; set; }

		[JsonProperty("token_use")] public string TokenUse { get; set; }

		[JsonProperty("auth_time")] public long AuthTime { get; set; }

		[JsonProperty("iss")] public string Iss { get; set; }

		[JsonProperty("preferred_username")] public string Username { get; set; }

		[JsonProperty("exp")] public long Exp { get; set; }

		[JsonProperty("iat")] public long Iat { get; set; }

		[JsonProperty("email")] public string Email { get; set; }
	}
}