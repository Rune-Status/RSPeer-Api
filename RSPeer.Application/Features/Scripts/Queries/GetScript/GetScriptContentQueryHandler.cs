using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Scripts.Queries.GetScript
{
	public class GetScriptContentQueryHandler : IRequestHandler<GetScriptContentQuery, ScriptContent>
	{
		private readonly RsPeerContext _db;

		public GetScriptContentQueryHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<ScriptContent> Handle(GetScriptContentQuery request, CancellationToken cancellationToken)
		{
			return await _db.ScriptContents.FirstOrDefaultAsync(w => w.ScriptId == request.ScriptId,
				cancellationToken);
		}
	}
}