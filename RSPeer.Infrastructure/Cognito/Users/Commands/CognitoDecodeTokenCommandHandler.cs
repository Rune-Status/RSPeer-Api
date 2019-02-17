using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RSPeer.Infrastructure.Cognito.Users.Base;
using RSPeer.Infrastructure.Cognito.Users.Models;

namespace RSPeer.Infrastructure.Cognito.Users.Commands
{
	public class CognitoDecodeTokenCommandHandler : BaseCognitoHandler<CognitoDecodeTokenCommand, string>
	{
		private readonly IMemoryCache _cache;
		private readonly HttpClient _client;
		private readonly string _region = "us-east-1";

		private readonly string _signingRouteBase =
			"https://cognito-idp.{location}.amazonaws.com/{userPoolId}/.well-known/jwks.json";

		public CognitoDecodeTokenCommandHandler(IHttpClientFactory factory, IMemoryCache cache, IConfiguration configuration) 
			: base(configuration)
		{
			_client = factory.CreateClient();
			_cache = cache;
		}

		private string FormattedSigningRoute =>
			_signingRouteBase.Replace("{location}", _region).Replace("{userPoolId}", UserPoolId);

		public override async Task<string> Handle(CognitoDecodeTokenCommand request,
			CancellationToken cancellationToken)
		{
			var token = request.Token;
			if (string.IsNullOrEmpty(token))
				throw new Exception("Invalid JWT.");

			var parts = token.Split(".");

			if (parts.Length != 3)
				throw new Exception("Invalid JWT.");

			var authenticationHeader = Encoding.UTF8.GetString(Base64UrlDecode(parts[0]));
			var authHeaderToDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(authenticationHeader);

			if (!authHeaderToDict.ContainsKey("kid"))
				throw new Exception("Unable to find Kid, invalid JWT.");

			var kid = authHeaderToDict["kid"];

			var keys = await GetKeys();

			if (keys == null)
				throw new Exception("Unable to get decryption keys. Can not decrypt JWT.");

			var correctKey = keys.Keys.FirstOrDefault(w => w.Kid == kid);

			if (correctKey == null)
				throw new Exception("Unable to get the right key from the kid. Can not decrypt JWT.");

			var rsa = new RSACryptoServiceProvider();

			rsa.ImportParameters(new RSAParameters
			{
				Modulus = Base64UrlDecode(correctKey.N),
				Exponent = Base64UrlDecode(correctKey.E)
			});

			var sha256 = SHA256.Create();

			var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(parts[0] + '.' + parts[1]));

			var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
			rsaDeformatter.SetHashAlgorithm("SHA256");

			if (!rsaDeformatter.VerifySignature(hash, Base64UrlDecode(parts[2])))
				throw new Exception("Invalid JWT.");

			var decoded = Encoding.UTF8.GetString(Base64UrlDecode(parts[1]));

			var user = JsonConvert.DeserializeObject<UserFromJwt>(decoded);

			var signingRouteFixed = FormattedSigningRoute.Replace("/.well-known/jwks.json", "");

			if (!string.Equals(user.Iss, signingRouteFixed, StringComparison.CurrentCultureIgnoreCase))
				throw new Exception("Invalid JWT.");

			return user.Email;
		}

		private async Task<JwtDecryptionKeys> GetKeys()
		{
			return await _cache.GetOrCreateAsync("cognito_jwt_decrypt_keys", async entry =>
			{
				entry.SlidingExpiration = TimeSpan.FromHours(24);
				var data = await _client.GetStringAsync(FormattedSigningRoute);
				return JsonConvert.DeserializeObject<JwtDecryptionKeys>(data);
			});
		}

		private static byte[] Base64UrlDecode(string arg)
		{
			var s = arg;
			s = s.Replace('-', '+'); // 62nd char of encoding
			s = s.Replace('_', '/'); // 63rd char of encoding
			switch (s.Length % 4) // Pad with trailing '='s
			{
				case 0: break; // No pad chars in this case
				case 2:
					s += "==";
					break; // Two pad chars
				case 3:
					s += "=";
					break; // One pad char
				default:
					throw new Exception(
						"Illegal base64url string!");
			}

			return Convert.FromBase64String(s); // Standard base64 decoder
		}
	}
}