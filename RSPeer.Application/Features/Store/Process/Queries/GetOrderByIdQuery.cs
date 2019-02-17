using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Store.Process.Queries
{
	public class GetOrderByIdQuery : IRequest<Order>
	{
		public int OrderId { get; set; }
	}
}