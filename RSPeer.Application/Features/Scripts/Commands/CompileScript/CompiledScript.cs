using MediatR;

namespace RSPeer.Application.Features.Scripts.Commands.CompileScript
{
	public class CompiledScript : IRequest
	{
		public byte[] Content { get; set; }
	}
}