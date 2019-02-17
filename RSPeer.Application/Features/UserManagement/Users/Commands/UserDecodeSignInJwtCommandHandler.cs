using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jose;
using MediatR;
using Microsoft.Extensions.Configuration;
using RSPeer.Application.Features.UserManagement.Users.Models;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.UserManagement.Users.Commands
{
	public class UserDecodeSignInJwtCommandHandler : IRequestHandler<UserDecodeSignInJwtCommand, User>
	{
		private readonly byte[] _secret; 

		public UserDecodeSignInJwtCommandHandler(IConfiguration configuration)
		{
			_secret = Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Secret"));
		}

		public Task<User> Handle(UserDecodeSignInJwtCommand request, CancellationToken cancellationToken)
		{
			var result = JWT.Decode<UserSignInResultPayload>(request.Token, _secret, JwsAlgorithm.HS512);
			return Task.FromResult(result.User);
		}
	}
}