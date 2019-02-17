using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Scripts.Queries.GetScript
{
	public class GetScriptByIdQueryHandler : IRequestHandler<GetScriptByIdQuery, Script>
	{
		private readonly RsPeerContext _db;

		public GetScriptByIdQueryHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<Script> Handle(GetScriptByIdQuery request, CancellationToken cancellationToken)
		{
			return await _db.Scripts.FirstOrDefaultAsync(w => w.Id == request.ScriptId,
				cancellationToken);
		}
	}
}