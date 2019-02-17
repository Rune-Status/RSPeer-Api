using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace RSPeer.Api.Migrations
{
	public partial class InitialCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				"balancechanges",
				table => new
				{
					id = table.Column<int>(nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
					userid = table.Column<int>(nullable: false),
					timestamp = table.Column<DateTimeOffset>(nullable: false),
					oldbalance = table.Column<int>(nullable: false),
					newbalance = table.Column<int>(nullable: false),
					adminuserid = table.Column<int>(nullable: false),
					orderid = table.Column<int>(nullable: false)
				},
				constraints: table => { table.PrimaryKey("PK_balancechanges", x => x.id); });

			migrationBuilder.CreateTable(
				"groups",
				table => new
				{
					id = table.Column<int>(nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
					name = table.Column<string>(nullable: false),
					description = table.Column<string>(nullable: true)
				},
				constraints: table => { table.PrimaryKey("PK_groups", x => x.id); });

			migrationBuilder.CreateTable(
				"items",
				table => new
				{
					id = table.Column<int>(nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
					name = table.Column<string>(nullable: false),
					description = table.Column<string>(nullable: true),
					sku = table.Column<string>(nullable: false),
					price = table.Column<decimal>(nullable: false),
					paymentmethod = table.Column<int>(nullable: false)
				},
				constraints: table => { table.PrimaryKey("PK_items", x => x.id); });

			migrationBuilder.CreateTable(
				"runescapeclients",
				table => new
				{
					id = table.Column<int>(nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
					userid = table.Column<int>(nullable: false),
					lastupdate = table.Column<DateTimeOffset>(nullable: false),
					ip = table.Column<string>(nullable: true),
					proxyip = table.Column<string>(nullable: true),
					machinename = table.Column<string>(nullable: true),
					operatingsystem = table.Column<string>(nullable: true),
					scriptname = table.Column<string>(nullable: true),
					rsn = table.Column<string>(nullable: true),
					runescapeemail = table.Column<string>(nullable: true),
					ismanuallyclosed = table.Column<bool>(nullable: false),
					tag = table.Column<Guid>(nullable: false)
				},
				constraints: table => { table.PrimaryKey("PK_runescapeclients", x => x.id); });

			migrationBuilder.CreateTable(
				"scripts",
				table => new
				{
					id = table.Column<int>(nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
					userid = table.Column<int>(nullable: false),
					name = table.Column<string>(nullable: false),
					description = table.Column<string>(nullable: false),
					version = table.Column<decimal>(nullable: false),
					type = table.Column<int>(nullable: false),
					status = table.Column<int>(nullable: false),
					price = table.Column<decimal>(nullable: true),
					instances = table.Column<int>(nullable: true),
					maxusers = table.Column<int>(nullable: true),
					category = table.Column<int>(nullable: false),
					forumthread = table.Column<string>(nullable: false),
					lastupdate = table.Column<DateTimeOffset>(nullable: false),
					totalusers = table.Column<int>(nullable: false, defaultValue: 0)
				},
				constraints: table => { table.PrimaryKey("PK_scripts", x => x.id); });

			migrationBuilder.CreateTable(
				"users",
				table => new
				{
					id = table.Column<int>(nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
					username = table.Column<string>(nullable: true),
					email = table.Column<string>(nullable: true),
					balance = table.Column<int>(nullable: false),
					isemailverified = table.Column<bool>(nullable: false)
				},
				constraints: table => { table.PrimaryKey("PK_users", x => x.id); });

			migrationBuilder.CreateTable(
				"orders",
				table => new
				{
					id = table.Column<int>(nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
					userid = table.Column<int>(nullable: false),
					paypalid = table.Column<string>(nullable: true),
					total = table.Column<decimal>(nullable: false),
					quantity = table.Column<int>(nullable: false),
					isrefunded = table.Column<bool>(nullable: false),
					itemid = table.Column<int>(nullable: false),
					timestamp = table.Column<DateTimeOffset>(nullable: false),
					status = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_orders", x => x.id);
					table.ForeignKey(
						"FK_orders_items_itemid",
						x => x.itemid,
						"items",
						"id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				"scriptcontents",
				table => new
				{
					id = table.Column<int>(nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
					scriptid = table.Column<int>(nullable: false),
					content = table.Column<byte[]>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_scriptcontents", x => x.id);
					table.ForeignKey(
						"FK_scriptcontents_scripts_scriptid",
						x => x.scriptid,
						"scripts",
						"id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				"usergroups",
				table => new
				{
					groupid = table.Column<int>(nullable: false),
					userid = table.Column<int>(nullable: false),
					id = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_usergroups", x => new { x.groupid, x.userid });
					table.ForeignKey(
						"FK_usergroups_groups_groupid",
						x => x.groupid,
						"groups",
						"id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						"FK_usergroups_users_userid",
						x => x.userid,
						"users",
						"id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				"IX_orders_itemid",
				"orders",
				"itemid");

			migrationBuilder.CreateIndex(
				"IX_scriptcontents_scriptid",
				"scriptcontents",
				"scriptid",
				unique: true);

			migrationBuilder.CreateIndex(
				"IX_usergroups_userid",
				"usergroups",
				"userid");

			migrationBuilder.CreateIndex(
				"IX_users_email",
				"users",
				"email");

			migrationBuilder.CreateIndex(
				"IX_users_username",
				"users",
				"username");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				"balancechanges");

			migrationBuilder.DropTable(
				"orders");

			migrationBuilder.DropTable(
				"runescapeclients");

			migrationBuilder.DropTable(
				"scriptcontents");

			migrationBuilder.DropTable(
				"usergroups");

			migrationBuilder.DropTable(
				"items");

			migrationBuilder.DropTable(
				"scripts");

			migrationBuilder.DropTable(
				"groups");

			migrationBuilder.DropTable(
				"users");
		}
	}
}