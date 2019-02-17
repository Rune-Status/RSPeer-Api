using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RSPeer.Application.Features.Scripts.Queries.GetScript;
using RSPeer.Application.Features.Store.Purchase.Models;
using RSPeer.Application.Features.UserManagement.Users.Commands;
using RSPeer.Common.Enums;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Store.Process.Commands.Children
{
	public class ProcessScriptPurchaseCommandHandler : IRequestHandler<ProcessScriptPurchaseCommand, PurchaseItemResult>
	{
		private readonly RsPeerContext _db;
		private readonly IMediator _mediator;

		public ProcessScriptPurchaseCommandHandler(IMediator mediator, RsPeerContext db)
		{
			_mediator = mediator;
			_db = db;
		}

		public async Task<PurchaseItemResult> Handle(ProcessScriptPurchaseCommand request,
			CancellationToken cancellationToken)
		{
			var scriptId = int.Parse(request.Item.Sku.Replace("premium-script-", ""));
			var script = await _mediator.Send(new GetScriptByIdQuery { ScriptId = scriptId }, cancellationToken);
			var isBuyingOwnScript = request.Order.UserId == script.UserId;

			if (!isBuyingOwnScript)
				await _mediator.Send(new UserUpdateBalanceCommand
						{ UserId = script.UserId, Amount = (int) request.Order.Total, Type = AddRemove.Add },
					cancellationToken);

			await _db.ScriptAccess.AddAsync(new ScriptAccess
			{
				Expiration = DateTimeOffset.UtcNow.AddDays(30),
				Instances = script.Instances,
				OrderId = request.Order.Id,
				UserId = request.Order.UserId,
				Timestamp = DateTimeOffset.UtcNow,
				ScriptId = script.Id,
				Recurring = request.Order.Recurring
			}, cancellationToken);

			request.Order.Status = OrderStatus.Completed;
			_db.Orders.Update(request.Order);

			await _db.SaveChangesAsync(cancellationToken);

			return new PurchaseItemResult
			{
				Status = OrderStatus.Completed,
				PaymentMethod = PaymentMethod.Tokens,
				Sku = request.Item.Sku,
				Total = request.Order.Total,
				IsCreator = isBuyingOwnScript
			};
		}
	}
}