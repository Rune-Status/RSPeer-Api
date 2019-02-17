using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Application.Features.Instance.Queries.Shared;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Instance.Queries
{
	public class GetRunningClientsCountQueryHandler : IRequestHandler<GetRunningClientsCountQuery, int>
	{
		private readonly RsPeerContext _db;

		public GetRunningClientsCountQueryHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<int> Handle(GetRunningClientsCountQuery request, CancellationToken cancellationToken)
		{
			return await _db.RunningRunescapeClients(request.UserId).CountAsync(cancellationToken);
		}
	}
}