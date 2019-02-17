using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Application.Features.Instance.Queries.Shared;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Instance.Queries
{
	public class GetRunningClientsQueryHandler : IRequestHandler<GetRunningClientsQuery, IEnumerable<RunescapeClient>>
	{
		private readonly RsPeerContext _db;

		public GetRunningClientsQueryHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<IEnumerable<RunescapeClient>> Handle(GetRunningClientsQuery request,
			CancellationToken cancellationToken)
		{
			return await _db.RunningRunescapeClients(request.UserId).ToListAsync(cancellationToken);
		}
	}
}