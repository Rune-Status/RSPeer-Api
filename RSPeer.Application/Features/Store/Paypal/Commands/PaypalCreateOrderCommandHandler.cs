using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RSPeer.Application.Features.Store.Paypal.Commands.Base;
using RSPeer.Application.Features.Store.Paypal.Models;

namespace RSPeer.Application.Features.Store.Paypal.Commands
{
	public class
		PaypalCreateOrderCommandHandler : BasePaypalCommandHandler<PaypalCreateOrderCommand, PaypalCreateOrderResult>
	{
		private readonly string _baseUrl;

		public PaypalCreateOrderCommandHandler(IHttpClientFactory factory, IMediator mediator, IConfiguration configuration) 
			: base(factory, mediator)
		{
			_baseUrl = configuration.GetValue<string>("Paypal:BaseUrl");
		}

		public override async Task<PaypalCreateOrderResult> Handle(PaypalCreateOrderCommand request,
			CancellationToken cancellationToken)
		{
			var order = BuildOrder(request);
			var body = await MakeAuthorizedPost(_baseUrl, "payments/payment", order);
			var created = JsonConvert.DeserializeObject<PaypalCreatedOrder>(body);
			created.UserId = request.User.Id;
			var checkoutUrl = created.Links.Where(w => w.Method.ToLower() == "redirect").Select(w => w.Href)
				.FirstOrDefault();

			return new PaypalCreateOrderResult
			{
				Url = checkoutUrl,
				CreatedBy = request.User,
				Timestamp = DateTimeOffset.UtcNow,
				Total = request.Total,
				PaypalId = created.PaypalId
			};
		}

		private PaypalOrder BuildOrder(PaypalCreateOrderCommand request)
		{
			var itemPrice = request.Total / request.Quantity;
			return new PaypalOrder
			{
				Intent = "sale",
				Payer = new Payer
				{
					PaymentMethod = "paypal"
				},
				Transactions = new List<Transaction>
				{
					new Transaction
					{
						Description = request.Description,
						ItemList = new ItemList
						{
							Items = new List<Item>
							{
								new Item
								{
									Sku = request.Sku,
									Currency = "USD",
									Description = request.Description,
									Name = request.Name,
									Price = itemPrice.ToString(CultureInfo.InvariantCulture),
									Quantity = request.Quantity.ToString(),
									Tax = "0.00"
								}
							}
						},
						Amount = new Amount
						{
							Currency = "USD",
							Total = request.Total.ToString(CultureInfo.InvariantCulture),
							Details = new Details
							{
								HandlingFee = "0.00",
								Insurance = "0.00",
								Shipping = "0.00",
								Subtotal = request.Total.ToString(CultureInfo.InvariantCulture),
								Tax = "0.00"
							}
						}
					}
				},
				NoteToPayer = $"{request.Quantity} {request.Name}",
				RedirectUrls = new RedirectUrls
				{
					CancelUrl = request.RedirectUrlCancel,
					ReturnUrl = request.RedirectUrlSuccess
				}
			};
		}
	}
}