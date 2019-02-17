using MediatR;

namespace RSPeer.Application.Features.Instance.Queries
{
	public class GetRunningClientsCountQuery : IRequest<int>
	{
		public int UserId { get; set; }
	}
}