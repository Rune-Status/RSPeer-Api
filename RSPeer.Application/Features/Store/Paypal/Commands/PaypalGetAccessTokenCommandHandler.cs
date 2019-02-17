using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RSPeer.Application.Features.Store.Paypal.Commands
{
	public class PaypalGetAccessTokenCommandHandler : IRequestHandler<PaypalGetAccessTokenCommand, string>
	{
		private readonly HttpClient _client;
		private readonly string _clientId;
		private readonly string _secret;

		public PaypalGetAccessTokenCommandHandler(IHttpClientFactory factory, IConfiguration configuration)
		{
			_client = factory.CreateClient();
			_clientId = configuration.GetValue<string>("Paypal:ClientId");
			_secret = configuration.GetValue<string>("Paypal:Secret");
		}

		public async Task<string> Handle(PaypalGetAccessTokenCommand request, CancellationToken cancellationToken)
		{
			var token = Convert.ToBase64String(
				Encoding.ASCII.GetBytes(
					$"{_clientId}:{_secret}"));
			var header = "Basic " + token;
			var res = await _client.SendAsync(new HttpRequestMessage
			{
				Headers =
				{
					{ "Authorization", header }
				},
				Method = HttpMethod.Post,
				Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>("grant_type", "client_credentials")
				}),
				RequestUri = new Uri($"{request.BaseUrl}oauth2/token")
			}, cancellationToken);
			var body = await res.Content.ReadAsStringAsync();
			if (!res.IsSuccessStatusCode)
				throw new Exception("Failed to generate paypal access token.");
			var o = JsonConvert.DeserializeObject<JObject>(body);
			return o.ContainsKey("access_token") ? o.Value<string>("access_token") : null;
		}
	}
}