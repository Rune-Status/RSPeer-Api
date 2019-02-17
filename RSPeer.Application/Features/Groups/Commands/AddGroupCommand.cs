using MediatR;

namespace RSPeer.Application.Features.Groups.Commands
{
	public class AddGroupCommand : IRequest<int>
	{
		public string Name { get; set; }
		public string Description { get; set; }
	}
}