using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Scripts.Queries.GetScript
{
	public class GetScriptByNameQueryHandler : IRequestHandler<GetScriptByNameQuery, Script>
	{
		private readonly RsPeerContext _db;

		public GetScriptByNameQueryHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<Script> Handle(GetScriptByNameQuery request, CancellationToken cancellationToken)
		{
			return await _db.Scripts.FirstOrDefaultAsync(w => w.Name.ToLower() == request.Name.ToLower().Trim(),
				cancellationToken);
		}
	}
}