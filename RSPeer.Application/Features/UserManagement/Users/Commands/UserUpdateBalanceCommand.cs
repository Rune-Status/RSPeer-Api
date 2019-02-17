using MediatR;
using RSPeer.Common.Enums;

namespace RSPeer.Application.Features.UserManagement.Users.Commands
{
	public class UserUpdateBalanceCommand : IRequest<int>
	{
		public int UserId { get; set; }
		public int AdminUserId { get; set; }
		public int Amount { get; set; }
		public AddRemove Type { get; set; }
		public int OrderId { get; set; }
	}
}