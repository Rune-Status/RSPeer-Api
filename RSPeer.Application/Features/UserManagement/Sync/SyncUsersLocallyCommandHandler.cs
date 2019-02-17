using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Domain.Entities;
using RSPeer.Infrastructure.Cognito.Users.Queries;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.UserManagement.Sync
{
	public class SyncUsersLocallyCommandHandler : AsyncRequestHandler<SyncUsersLocallyCommand>
	{
		private readonly RsPeerContext _db;
		private readonly IMediator _mediator;

		public SyncUsersLocallyCommandHandler(RsPeerContext db, IMediator mediator)
		{
			_db = db;
			_mediator = mediator;
		}

		protected override async Task Handle(SyncUsersLocallyCommand request, CancellationToken cancellationToken)
		{
			await _mediator.Send(new CognitoListUsersQuery
			{
				Action = async users =>
				{
					var dict = new Dictionary<string, User>();
					foreach (var user in users) dict[user.Email] = user;
					var emails = dict.Keys.ToList();
					var existing = await _db.Users.Where(p => emails.Contains(p.Email)).Select(w => w.Email)
						.ToListAsync(cancellationToken);
					foreach (var email in existing) dict.Remove(email);
					foreach (var pair in dict) _db.Users.Add(pair.Value);
					await _db.SaveChangesAsync(cancellationToken);
				}
			}, cancellationToken);
		}
	}
}