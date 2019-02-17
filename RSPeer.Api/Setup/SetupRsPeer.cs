using System.Linq;
using Microsoft.EntityFrameworkCore;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Api.Setup
{
	public class SetupRsPeer
	{
		private readonly RsPeerContext _db;

		public SetupRsPeer(RsPeerContext db)
		{
			_db = db;
		}

		public void Execute()
		{
			var tokens = _db.Items.FirstOrDefault(w => w.Sku == "tokens");
			if (tokens == null)
			{
				_db.Items.Add(new Item
				{
					Name = "Tokens",
					Sku = "tokens",
					Description = "RSPeer Tokens, Used to purchase instances, scripts, and ranks."
				});
			}
			
			var owners = _db.Groups.FirstOrDefault(w => w.Name == "Owners");
			if (owners == null)
			{
				_db.Groups.Add(new Group
				{
					Description = "The owners of RSPeer.",
					Name = "Owners"
				});
			}

			_db.SaveChanges();
		}
	}
}