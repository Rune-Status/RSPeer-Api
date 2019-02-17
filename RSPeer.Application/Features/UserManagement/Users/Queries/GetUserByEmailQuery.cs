using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.UserManagement.Users.Queries
{
	public class GetUserByEmailQuery : IRequest<User>
	{
		public string Email { get; set; }
		public bool IncludeGroups { get; set; }
	}
}