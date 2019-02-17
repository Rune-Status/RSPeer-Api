using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Store.Items.Queries
{
	public class GetItemByIdQuery : IRequest<Item>
	{
		public int Id { get; set; }
	}
}