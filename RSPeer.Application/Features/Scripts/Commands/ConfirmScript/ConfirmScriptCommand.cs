using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Scripts.Commands.ConfirmScript
{
	public class ConfirmScriptCommand : IRequest<Unit>
	{
		public User User { get; set; }
		public int ScriptId { get; set; }
	}
}