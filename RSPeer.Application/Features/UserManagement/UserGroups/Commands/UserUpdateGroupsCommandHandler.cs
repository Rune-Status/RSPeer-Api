using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Application.Exceptions;
using RSPeer.Common.Enums;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.UserManagement.UserGroups.Commands
{
	public class UserUpdateGroupsCommandHandler : IRequestHandler<UserUpdateGroupsCommand, Unit>
	{
		private readonly RsPeerContext _db;

		public UserUpdateGroupsCommandHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<Unit> Handle(UserUpdateGroupsCommand request, CancellationToken cancellationToken)
		{
			var user = await _db.Users.Where(w => w.Id == request.UserId).Include(w => w.UserGroups)
				.FirstOrDefaultAsync(cancellationToken);

			if (user == null) throw new UserNotFoundException(request.UserId);

			switch (request.Type)
			{
				case AddRemove.Add when user.UserGroups.FirstOrDefault(w => w.GroupId == request.GroupId) != null:
					return Unit.Value;
				case AddRemove.Remove:
					user.UserGroups.Remove(user.UserGroups.FirstOrDefault(w => w.GroupId == request.GroupId));
					break;
				default:
					user.UserGroups.Add(new UserGroup { GroupId = request.GroupId, UserId = request.UserId });
					break;
			}

			await _db.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}