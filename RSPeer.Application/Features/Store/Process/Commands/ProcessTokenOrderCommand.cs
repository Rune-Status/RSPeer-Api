using MediatR;
using RSPeer.Application.Features.Store.Purchase.Models;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Store.Process.Commands
{
	public class ProcessTokenOrderCommand : IRequest<PurchaseItemResult>
	{
		public Order Order { get; set; }
		public Item Item { get; set; }
		public User User { get; set; }
	}
}