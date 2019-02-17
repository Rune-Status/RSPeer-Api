using System.Collections.Generic;
using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Instance.Queries
{
	public class GetRunningClientsQuery : IRequest<IEnumerable<RunescapeClient>>
	{
		public int UserId { get; set; }
	}
}