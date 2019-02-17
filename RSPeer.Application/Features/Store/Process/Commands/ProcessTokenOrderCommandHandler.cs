using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RSPeer.Application.Features.Store.Process.Commands.Children;
using RSPeer.Application.Features.Store.Purchase.Models;
using RSPeer.Application.Features.UserManagement.Users.Commands;
using RSPeer.Application.Features.UserManagement.Users.Queries;
using RSPeer.Common.Enums;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Store.Process.Commands
{
	public class ProcessTokenOrderCommandHandler : IRequestHandler<ProcessTokenOrderCommand, PurchaseItemResult>
	{
		private readonly RsPeerContext _db;
		private readonly IMediator _mediator;

		public ProcessTokenOrderCommandHandler(IMediator mediator, RsPeerContext db)
		{
			_mediator = mediator;
			_db = db;
		}

		public async Task<PurchaseItemResult> Handle(ProcessTokenOrderCommand request,
			CancellationToken cancellationToken)
		{
			await AssertHasBalance(request);

			using (var transaction = await _db.Database.BeginTransactionAsync(cancellationToken))
			{
				PurchaseItemResult result = null;
				if (request.Item.Sku.StartsWith("premium-script-"))
					result = await _mediator.Send(new ProcessScriptPurchaseCommand
					{
						Item = request.Item,
						Order = request.Order
					}, cancellationToken);

				await RemoveBalance(request, result);

				transaction.Commit();

				return result ?? new PurchaseItemResult
				{
					PaymentMethod = PaymentMethod.Tokens,
					Sku = request.Item.Sku,
					Status = OrderStatus.Completed,
					Total = request.Order.Total
				};
			}
		}

		private async Task RemoveBalance(ProcessTokenOrderCommand command, PurchaseItemResult result)
		{
			//If they are the creator of what they are purchasing, such as a premium script, do not charge them.
			if (result != null && result.IsCreator) return;

			await _mediator.Send(new UserUpdateBalanceCommand
			{
				Amount = (int) command.Item.Price, OrderId = command.Order.Id, UserId = command.User.Id,
				Type = AddRemove.Remove
			});
		}

		private async Task AssertHasBalance(ProcessTokenOrderCommand request)
		{
			var user = await _mediator.Send(new GetUserByIdQuery { Id = request.User.Id });
			if (user.Balance < request.Order.Total) throw new Exception("Insufficient funds.");
		}
	}
}