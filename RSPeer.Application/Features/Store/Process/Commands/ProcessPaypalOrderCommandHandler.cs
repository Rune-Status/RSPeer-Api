using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RSPeer.Application.Features.Store.Items.Queries;
using RSPeer.Application.Features.Store.Process.Commands.Children;

namespace RSPeer.Application.Features.Store.Process.Commands
{
	public class ProcessPaypalOrderCommandHandler : IRequestHandler<ProcessPaypalOrderCommand, Unit>
	{
		private readonly IMediator _mediator;

		public ProcessPaypalOrderCommandHandler(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<Unit> Handle(ProcessPaypalOrderCommand request, CancellationToken cancellationToken)
		{
			var item = await _mediator.Send(new GetItemByIdQuery { Id = request.Order.ItemId }, cancellationToken);
			if (item.Sku == "tokens")
				await _mediator.Send(new ProcessTokenPurchaseCommand { Order = request.Order },
					cancellationToken);
			return Unit.Value;
		}
	}
}