using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using RSPeer.Application.Exceptions;

namespace RSPeer.Application.Features.Store.Paypal.Commands.Base
{
	public abstract class BasePaypalCommandHandler<T, TU> : IRequestHandler<T, TU> where T : IRequest<TU>
	{
		private readonly HttpClient _client;
		private readonly IMediator _mediator;

		protected BasePaypalCommandHandler(IHttpClientFactory factory, IMediator mediator)
		{
			_client = factory.CreateClient("Paypal");
			_mediator = mediator;
		}

		public abstract Task<TU> Handle(T request, CancellationToken cancellationToken);

		protected async Task<string> MakeAuthorizedPost(string baseUrl, string path, object payload)
		{
			var token = await _mediator.Send(new PaypalGetAccessTokenCommand { BaseUrl = baseUrl });
			var res = await _client.SendAsync(new HttpRequestMessage
			{
				Headers =
				{
					{ "Authorization", "Bearer " + token }
				},
				Method = HttpMethod.Post,
				Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"),
				RequestUri = new Uri(baseUrl + path)
			});
			var body = await res.Content.ReadAsStringAsync();
			if (!res.IsSuccessStatusCode) throw new PaypalException(body);
			return body;
		}
	}
}