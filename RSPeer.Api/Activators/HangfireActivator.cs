using System;
using Hangfire;

namespace RSPeer.Api.Activators
{
	public class HangfireActivator : JobActivator
	{
		private readonly IServiceProvider _serviceProvider;

		public HangfireActivator(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public override object ActivateJob(Type type)
		{
			return _serviceProvider.GetService(type);
		}
	}
}