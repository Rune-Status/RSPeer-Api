using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace RSPeer.Application.Features.Scripts.Commands.CompileScript
{
	public class CompileScriptHandler : IRequestHandler<CompileScriptCommand, CompiledScript>
	{
		public Task<CompiledScript> Handle(CompileScriptCommand request, CancellationToken cancellationToken)
		{
			return Task.FromResult(new CompiledScript
			{
				Content = File.ReadAllBytes(Path.GetFullPath(request.GitlabUrl))
			});
		}
	}
}