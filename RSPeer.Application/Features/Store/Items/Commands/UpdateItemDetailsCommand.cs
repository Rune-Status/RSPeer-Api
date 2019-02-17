using MediatR;

namespace RSPeer.Application.Features.Store.Items.Commands
{
	public class UpdateItemDetailsCommand : IRequest<Unit>
	{
		public int ItemId { get; set; }
		public decimal Price { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
	}
}