using MediatR;

namespace RSPeer.Application.Features.Scripts.Commands.CompileScript
{
	public class CompileScriptCommand : IRequest<CompiledScript>
	{
		public string GitlabUrl { get; set; }
	}
}