using System;
using System.Linq;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Instance.Queries.Shared
{
	public static class QueryPredicates
	{
		public static IQueryable<RunescapeClient> RunningRunescapeClients(this RsPeerContext db, int userId)
		{
			var threshold = DateTimeOffset.UtcNow.AddMinutes(-1);
			return db.RunescapeClients.Where(client =>
				client.UserId == userId && !client.IsManuallyClosed && client.LastUpdate > threshold);
		}
	}
}