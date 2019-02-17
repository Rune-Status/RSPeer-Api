using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Store.Process.Commands.Children
{
	public class ProcessTokenPurchaseCommand : IRequest<Unit>
	{
		public Order Order { get; set; }
		public User User { get; set; }
	}
}