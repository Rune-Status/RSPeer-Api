using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Store.Items.Queries
{
	public class GetItemBySkuQuery : IRequest<Item>
	{
		public string Sku { get; set; }
	}
}