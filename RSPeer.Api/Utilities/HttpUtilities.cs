using Microsoft.AspNetCore.Http;

namespace RSPeer.Api.Utilities
{
	public class HttpUtilities
	{
		public static string GetIp(HttpRequest request)
		{
			return request.HttpContext.Connection.RemoteIpAddress
				.ToString();
		}

		public static string TryGetSession(HttpContext context)
		{
			var headers = context.Request.Headers;
			if (!headers.ContainsKey("Authorization"))
				return null;
			string header = context.Request.Headers["Authorization"];
			if (string.IsNullOrEmpty(header))
				return null;
			var token = header.Substring("bearer ".Length)?.Trim();
			token = string.IsNullOrEmpty(token) ? null : token;
			context.Items.Add("CurrentSession", token);
			return token;
		}
	}
}