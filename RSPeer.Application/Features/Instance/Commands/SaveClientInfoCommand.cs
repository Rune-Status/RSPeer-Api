using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Instance.Commands
{
	public class SaveClientInfoCommand : IRequest<Unit>
	{
		public RunescapeClient Client { get; set; }
	}
}