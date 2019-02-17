using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Application.Exceptions;
using RSPeer.Application.Features.Store.Process.Commands;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Store.Paypal.Commands
{
	public class PaypalFinishedOrderCommandHandler : IRequestHandler<PaypalFinishOrderCommand, Unit>
	{
		private readonly RsPeerContext _db;
		private readonly IMediator _mediator;

		public PaypalFinishedOrderCommandHandler(RsPeerContext db, IMediator mediator)
		{
			_db = db;
			_mediator = mediator;
		}

		public async Task<Unit> Handle(PaypalFinishOrderCommand request, CancellationToken cancellationToken)
		{
			if (request.Callback.ResourceType != "sale") return Unit.Value;

			var orderId = request.Callback.Resource.ParentPayment;

			var order = await _db.Orders.FirstOrDefaultAsync(w =>
				w.PaypalId == orderId, cancellationToken);

			if (order == null) throw new NotFoundException("PaypalOrderCallback", orderId);

			if (order.Status == OrderStatus.Completed || order.Status == OrderStatus.Processing)
				throw new Exception("Order is already processing or completed.");

			order.Status = OrderStatus.Processing;
			_db.Update(order);
			await _db.SaveChangesAsync(cancellationToken);

			await _mediator.Send(new ProcessPaypalOrderCommand { Order = order }, cancellationToken);

			return Unit.Value;
		}
	}
}