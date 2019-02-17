using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RSPeer.Application.Exceptions;
using RSPeer.Application.Features.Store.Process.Queries;
using RSPeer.Application.Features.UserManagement.Users.Commands;
using RSPeer.Common.Enums;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Store.Process.Commands.Children
{
	public class ProcessTokenPurchaseCommandHandler : IRequestHandler<ProcessTokenPurchaseCommand, Unit>
	{
		private readonly RsPeerContext _db;
		private readonly IMediator _mediator;

		public ProcessTokenPurchaseCommandHandler(RsPeerContext db, IMediator mediator)
		{
			_db = db;
			_mediator = mediator;
		}

		public async Task<Unit> Handle(ProcessTokenPurchaseCommand request, CancellationToken cancellationToken)
		{
			var order = await _mediator.Send(new GetOrderByIdQuery { OrderId = request.Order.Id }, cancellationToken);

			if (order == null || order.Status == OrderStatus.Completed)
				throw new NotFoundException("ProcessTokenPurchase_OrderNotFound", request.Order.Id);

			using (var transaction = await _db.Database.BeginTransactionAsync(cancellationToken))
			{
				order.Status = OrderStatus.Completed;
				_db.Update(order);
				await _db.SaveChangesAsync(cancellationToken);
				await _mediator.Send(
					new UserUpdateBalanceCommand
					{
						Amount = request.Order.Quantity, UserId = order.UserId, OrderId = order.Id, Type = AddRemove.Add
					}, cancellationToken);
				transaction.Commit();
				return Unit.Value;
			}
		}
	}
}