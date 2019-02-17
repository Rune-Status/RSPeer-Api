using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nelibur.ObjectMapper;
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

					foreach (var user in users)
					{
						dict[user.Email] = user;
					}
					
					var emails = dict.Keys.ToList();
					
					var existing = await _db.Users.Where(p => emails.Contains(p.Email))
						.ToListAsync(cancellationToken);

					var existingUsersDict = existing.ToDictionary(w => w.Email, w => w);
			
					foreach (var pair in dict)
					{
						if (existingUsersDict.ContainsKey(pair.Value.Email))
						{
							TinyMapper.Map(pair.Value, existingUsersDict[pair.Value.Email]);
							_db.Users.Update(existingUsersDict[pair.Value.Email]);
						}
						else
						{
							_db.Users.Add(pair.Value);				
						}
					}
					
					try
					{
						await _db.SaveChangesAsync(cancellationToken);
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
					}
				}
			}, cancellationToken);
		}
	}
}