using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Scripts.Queries.GetScript
{
	public class GetScriptByNameQuery : IRequest<Script>
	{
		public string Name { get; set; }
	}
}