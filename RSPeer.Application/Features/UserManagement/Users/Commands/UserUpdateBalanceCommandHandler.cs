using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Application.Exceptions;
using RSPeer.Common.Enums;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.UserManagement.Users.Commands
{
	public class UserUpdateBalanceCommandHandler : IRequestHandler<UserUpdateBalanceCommand, int>
	{
		private readonly RsPeerContext _db;

		public UserUpdateBalanceCommandHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<int> Handle(UserUpdateBalanceCommand request, CancellationToken cancellationToken)
		{
			var user = await _db.Users.FirstOrDefaultAsync(w => w.Id == request.UserId, cancellationToken);
			if (user == null) throw new UserNotFoundException(request.UserId);

			var balanceChange = new BalanceChange
			{
				OldBalance = user.Balance
			};

			switch (request.Type)
			{
				case AddRemove.Add:
					user.Balance += request.Amount;
					break;
				case AddRemove.Remove:
					user.Balance -= request.Amount;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			balanceChange.NewBalance = user.Balance;
			balanceChange.UserId = user.Id;
			balanceChange.Timestamp = DateTimeOffset.UtcNow;
			balanceChange.AdminUserId = request.AdminUserId;
			balanceChange.OrderId = request.OrderId;

			_db.Update(user);
			_db.BalanceChanges.Add(balanceChange);
			await _db.SaveChangesAsync(cancellationToken);
			return user.Balance;
		}
	}
}