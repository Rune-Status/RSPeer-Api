using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.UserManagement.Users.Queries
{
	public class GetUserByTokenQuery : IRequest<User>
	{
		public string Token { get; set; }
		public bool IncludeGroups { get; set; }
	}
}