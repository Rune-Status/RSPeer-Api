using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jose;
using MediatR;
using Microsoft.Extensions.Configuration;
using RSPeer.Application.Features.UserManagement.Users.Models;

namespace RSPeer.Application.Features.UserManagement.Users.Commands
{
	public class UserCreateSignInJwtCommandHandler : IRequestHandler<UserCreateSignInJwtCommand, UserSignInResult>
	{
		private readonly byte[] _secret; 

		public UserCreateSignInJwtCommandHandler(IConfiguration configuration)
		{
			_secret = Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Secret"));
		}

		public Task<UserSignInResult> Handle(UserCreateSignInJwtCommand request, CancellationToken cancellationToken)
		{
			var token = JWT.Encode(request.Payload, _secret, JwsAlgorithm.HS512);
			return Task.FromResult(new UserSignInResult
			{
				Token = token
			});
		}
	}
}