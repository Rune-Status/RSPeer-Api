using System.Reflection;
using AutoMapper;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RSPeer.Api.Activators;
using RSPeer.Api.Mappers;
using RSPeer.Application.Features.Scripts.Commands.CreateScript;
using RSPeer.Application.Infrastructure;
using RSPeer.Application.Infrastructure.AutoMapper;
using RSPeer.Infrastructure.Cognito.Users.Commands;
using RSPeer.Persistence;

namespace RSPeer.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAutoMapper(typeof(AutoMapperProfile).GetTypeInfo().Assembly);

			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

			services.AddMediatR(typeof(CreateScriptCommandHandler).GetTypeInfo().Assembly);
			services.AddMediatR(typeof(CognitoSignUpCommand).GetTypeInfo().Assembly);

			services.AddHttpClient();

			services.AddEntityFrameworkNpgsql().AddDbContext<RsPeerContext>((provider, builder) =>
			{
				builder.UseNpgsql(Configuration.GetConnectionString("Postgres"),
					options => { options.MigrationsAssembly(typeof(Startup).Assembly.FullName); });
				builder.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
			});

			services.AddHangfire(config =>
				config.UsePostgreSqlStorage(Configuration.GetConnectionString("Postgres")));

			services.AddMemoryCache();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
				.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateScriptCommandHandler>());
			
			services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

			EntityMapperExtensions.AddEntityMappers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			else
				app.UseHsts();

			GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(app.ApplicationServices));

			app.UseHangfireServer();
			app.UseHangfireDashboard();

			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}