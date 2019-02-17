using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Store.Process.Commands
{
	public class ProcessPaypalOrderCommand : IRequest<Unit>
	{
		public Order Order { get; set; }
	}
}