using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RSPeer.Infrastructure.Forums.Users.Commands
{
	public class ForumsRegisterUserCommandHandler : RequestHandler<ForumsRegisterUserCommand>
	{
		private readonly string _apiPath;
		private readonly string _apiToken;

		private readonly HttpClient _client;

		public ForumsRegisterUserCommandHandler(IHttpClientFactory factory, IConfiguration configuration)
		{
			_client = factory.CreateClient();
			_apiToken = configuration.GetValue<string>("Forums:Token");
			_apiPath = configuration.GetValue<string>("Forums:Url");
		}

		protected override void Handle(ForumsRegisterUserCommand request)
		{
			BackgroundJob.Enqueue(() => Execute(request));
		}

		public async Task<int> Execute(ForumsRegisterUserCommand request)
		{
			var dict = new Dictionary<string, string> { { "username", request.Username }, { "email", request.Email } };
			var res = await _client.PostAsync($"{_apiPath}/register?rspeerToken={_apiToken}",
				new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/json"));

			var content = await res.Content.ReadAsStringAsync();

			if (!res.IsSuccessStatusCode || content.Contains("error")) throw new Exception(content);

			var parsed = JsonConvert.DeserializeObject<JObject>(content);

			if (!parsed.ContainsKey("userId")) throw new Exception(content);

			return parsed["userId"].Value<int>();
		}
	}
}