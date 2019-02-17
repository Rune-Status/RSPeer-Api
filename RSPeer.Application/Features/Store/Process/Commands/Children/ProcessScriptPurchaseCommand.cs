using MediatR;
using RSPeer.Application.Features.Store.Purchase.Models;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Store.Process.Commands.Children
{
	public class ProcessScriptPurchaseCommand : IRequest<PurchaseItemResult>
	{
		public Item Item { get; set; }
		public Order Order { get; set; }
	}
}