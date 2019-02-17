using System.Collections.Generic;
using Newtonsoft.Json;

namespace RSPeer.Infrastructure.Cognito.Users.Models
{
	public class JwtDecryptionKeys
	{
		[JsonProperty("keys")] public List<JwtDecryptionKey> Keys { get; set; }
	}

	public class JwtDecryptionKey
	{
		[JsonProperty("alg")] public string Alg { get; set; }

		[JsonProperty("e")] public string E { get; set; }

		[JsonProperty("kid")] public string Kid { get; set; }

		[JsonProperty("kty")] public string Kty { get; set; }

		[JsonProperty("n")] public string N { get; set; }

		[JsonProperty("use")] public string Use { get; set; }
	}
}