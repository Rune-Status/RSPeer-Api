using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RSPeer.Api.Setup;
using RSPeer.Persistence;

namespace RSPeer.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateWebHostBuilder(args).Build();
			using (var scope = host.Services.CreateScope())
			{
				try
				{
					var context = scope.ServiceProvider.GetService<RsPeerContext>();
					context.Database.Migrate();
					var setup = new SetupRsPeer(context);
					setup.Execute();
				}
				catch (Exception ex)
				{
					var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while migrating or initializing the database.");
				}
			}

			host.Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args)
		{
			return WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
		}
	}
}