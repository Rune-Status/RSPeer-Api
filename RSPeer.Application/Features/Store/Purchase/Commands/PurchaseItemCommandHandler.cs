using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RSPeer.Application.Features.Store.Items.Queries;
using RSPeer.Application.Features.Store.Paypal.Commands;
using RSPeer.Application.Features.Store.Process.Commands;
using RSPeer.Application.Features.Store.Purchase.Models;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Store.Purchase.Commands
{
	public class PurchaseItemCommandHandler : IRequestHandler<PurchaseItemCommand, PurchaseItemResult>
	{
		private readonly RsPeerContext _db;
		private readonly IMediator _mediator;

		public PurchaseItemCommandHandler(IMediator mediator, RsPeerContext db)
		{
			_mediator = mediator;
			_db = db;
		}

		public async Task<PurchaseItemResult> Handle(PurchaseItemCommand request, CancellationToken cancellationToken)
		{
			var item = await _mediator.Send(new GetItemBySkuQuery { Sku = request.Sku }, cancellationToken);
			var total = item.Price * request.Quantity;

			if (item.PaymentMethod == PaymentMethod.Paypal)
			{
				var result = await _mediator.Send(new PaypalCreateOrderCommand
				{
					Description = item.Description,
					Name = item.Name,
					Quantity = request.Quantity,
					RedirectUrlCancel = "https://store.rspeer.org",
					RedirectUrlSuccess = "https://store.rspeer.org",
					Sku = item.Sku,
					Total = total,
					User = request.User
				}, cancellationToken);

				await CreateOrder(request.User, item, total, request.Quantity, result.PaypalId);
				return BuildResult(item, OrderStatus.Created, result.Url, total);
			}

			var order = await CreateOrder(request.User, item, total, request.Quantity);
			return await _mediator.Send(new ProcessTokenOrderCommand
			{
				Order = order,
				Item = item,
				User = request.User
			}, cancellationToken);
		}

		private async Task<Order> CreateOrder(User user, Item item, decimal total, int quantity, string paypalId = null)
		{
			var order = new Order
			{
				ItemId = item.Id,
				Status = OrderStatus.Created,
				Timestamp = DateTimeOffset.UtcNow,
				Total = total,
				PaypalId = paypalId,
				UserId = user.Id,
				Quantity = quantity
			};
			_db.Orders.Add(order);
			await _db.SaveChangesAsync();
			return order;
		}

		private PurchaseItemResult BuildResult(Item item, OrderStatus status, string meta, decimal total)
		{
			return new PurchaseItemResult
			{
				Meta = meta,
				PaymentMethod = item.PaymentMethod,
				Sku = item.Sku,
				Status = status,
				Total = total
			};
		}
	}
}