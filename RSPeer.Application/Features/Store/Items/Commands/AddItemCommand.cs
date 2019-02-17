using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Store.Items.Commands
{
	public class AddItemCommand : IRequest<int>
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Sku { get; set; }
		public decimal Price { get; set; }
		public PaymentMethod PaymentMethod { get; set; }

		public bool Upsert { get; set; }
	}
}