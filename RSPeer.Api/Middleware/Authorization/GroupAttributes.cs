using Microsoft.AspNetCore.Mvc;

namespace RSPeer.Api.Middleware.Authorization
{
	public class OwnerAttribute : TypeFilterAttribute
	{
		public OwnerAttribute() : base(typeof(GroupAuthorizationFilter))
		{
			Arguments = new object[] { 1 };
		}
	}
}