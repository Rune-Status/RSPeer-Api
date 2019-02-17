using MediatR;
using RSPeer.Common.Enums;

namespace RSPeer.Application.Features.UserManagement.UserGroups.Commands
{
	public class UserUpdateGroupsCommand : IRequest<Unit>
	{
		public int UserId { get; set; }
		public int GroupId { get; set; }
		public AddRemove Type { get; set; }
	}
}