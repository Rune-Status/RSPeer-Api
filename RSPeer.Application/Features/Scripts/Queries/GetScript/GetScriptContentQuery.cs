using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Scripts.Queries.GetScript
{
	public class GetScriptContentQuery : IRequest<ScriptContent>
	{
		public int ScriptId { get; set; }
	}
}