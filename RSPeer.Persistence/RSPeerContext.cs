using System.Linq;
using Microsoft.EntityFrameworkCore;
using RSPeer.Domain.Entities;

namespace RSPeer.Persistence
{
	public class RsPeerContext : DbContext
	{
		public RsPeerContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Script> Scripts { get; set; }

		public DbSet<ScriptAccess> ScriptAccess { get; set; }

		public DbSet<PendingScript> PendingScripts { get; set; }

		public DbSet<ScriptContent> ScriptContents { get; set; }

		public DbSet<User> Users { get; set; }

		public DbSet<UserGroup> UserGroups { get; set; }

		public DbSet<Group> Groups { get; set; }

		public DbSet<BalanceChange> BalanceChanges { get; set; }

		public DbSet<RunescapeClient> RunescapeClients { get; set; }

		public DbSet<Order> Orders { get; set; }

		public DbSet<Item> Items { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Model.GetEntityTypes()
				.Select(e => e.Relational())
				.ToList()
				.ForEach(t => t.TableName = t.TableName.ToLower());

			builder.Model.GetEntityTypes()
				.SelectMany(e => e.GetProperties())
				.ToList()
				.ForEach(c => c.Relational().ColumnName = c.Name.ToLower());

			builder.Entity<UserGroup>()
				.HasKey(t => new { t.GroupId, t.UserId });

			builder.Entity<UserGroup>()
				.HasOne(bc => bc.User)
				.WithMany(b => b.UserGroups)
				.HasForeignKey(bc => bc.UserId);

			builder.Entity<UserGroup>()
				.HasOne(bc => bc.Group)
				.WithMany(c => c.UserGroups)
				.HasForeignKey(bc => bc.GroupId);

			builder
				.Entity<User>()
				.HasIndex(w => w.Username);

			builder.Entity<User>()
				.HasIndex(w => w.Email);

			builder.Entity<RunescapeClient>().Property(w => w.UserId).IsRequired();
			builder.Entity<RunescapeClient>().Property(w => w.LastUpdate).IsRequired();

			builder.Entity<Group>().Property(w => w.Name).IsRequired();
			builder.Entity<BalanceChange>().Property(w => w.UserId).IsRequired();
			builder.Entity<BalanceChange>().Property(w => w.Timestamp).IsRequired();
			builder.Entity<BalanceChange>().Property(w => w.OldBalance).IsRequired();
			builder.Entity<BalanceChange>().Property(w => w.NewBalance).IsRequired();

			builder.Entity<Item>().Property(w => w.Sku).IsRequired();
			builder.Entity<Item>().Property(w => w.Name).IsRequired();
			builder.Entity<Item>().Property(w => w.PaymentMethod).IsRequired();
			builder.Entity<Item>().Property(w => w.Price).IsRequired();

			builder.Entity<Order>().Property(w => w.Total).IsRequired();
			builder.Entity<Order>().Property(w => w.Timestamp).IsRequired();
			builder.Entity<Order>().Property(w => w.UserId).IsRequired();
			builder.Entity<Order>().Property(w => w.ItemId).IsRequired();

			builder.Entity<Order>()
				.HasOne(p => p.Item)
				.WithMany(b => b.Orders)
				.HasForeignKey(p => p.ItemId);

			builder.Entity<ScriptContent>()
				.HasOne(p => p.Script)
				.WithOne(p => p.ScriptContent);

			builder.Entity<Script>()
				.HasOne(p => p.ScriptContent)
				.WithOne(p => p.Script);

			builder.Entity<Script>()
				.HasIndex(p => new { p.Name, p.Status }).IsUnique();

			builder.Entity<Script>().Property(w => w.Name).IsRequired();
			builder.Entity<Script>().Property(w => w.Description).IsRequired();
			builder.Entity<Script>().Property(w => w.UserId).IsRequired();
			builder.Entity<Script>().Property(w => w.Status).IsRequired();
			builder.Entity<Script>().Property(w => w.Type).IsRequired();
			builder.Entity<Script>().Property(w => w.Version).IsRequired();
			builder.Entity<Script>().Property(w => w.Category).IsRequired();
			builder.Entity<Script>().Property(w => w.ForumThread).IsRequired();
			builder.Entity<Script>().Property(w => w.TotalUsers).IsRequired().HasDefaultValue(0);

			base.OnModelCreating(builder);
		}
	}
}