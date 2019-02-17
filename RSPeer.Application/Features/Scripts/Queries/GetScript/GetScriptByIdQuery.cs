using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Scripts.Queries.GetScript
{
	public class GetScriptByIdQuery : IRequest<Script>
	{
		public int ScriptId { get; set; }
	}
}