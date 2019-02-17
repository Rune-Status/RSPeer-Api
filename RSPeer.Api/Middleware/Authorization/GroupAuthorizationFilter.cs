using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RSPeer.Api.Utilities;
using RSPeer.Application.Features.UserManagement.Users.Queries;

namespace RSPeer.Api.Middleware.Authorization
{
	public class GroupAuthorizationFilter : IAsyncAuthorizationFilter
	{
		private readonly IMediator _mediator;

		public GroupAuthorizationFilter(int groupId, IMediator mediator)
		{
			_mediator = mediator;
			GroupId = groupId;
		}

		private int GroupId { get; }

		public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
		{
			context.HttpContext.Items.Remove("CurrentUser");
			var session = HttpUtilities.TryGetSession(context.HttpContext);
			var user = await _mediator.Send(new GetUserByTokenQuery { Token = session, IncludeGroups = true });
			context.HttpContext.Items["CurrentUser"] = user;
			var hasGroup = user.Groups.Any(w => w.Id == GroupId);
			if (!hasGroup) context.Result = new UnauthorizedResult();
		}
	}
}