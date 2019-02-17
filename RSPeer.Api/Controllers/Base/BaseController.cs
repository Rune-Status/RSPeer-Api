using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RSPeer.Api.Utilities;
using RSPeer.Application.Features.UserManagement.Users.Queries;
using RSPeer.Domain.Entities;

namespace RSPeer.Api.Controllers.Base
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public abstract class BaseController : Controller
	{
		private IMediator _mediator;

		protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());


		public async Task<User> GetUser()
		{
			if (HttpContext.Items.ContainsKey("CurrentUser")) return (User) HttpContext.Items["CurrentUser"];
			var token = HttpUtilities.TryGetSession(HttpContext);
			return token == null ? null : await Mediator.Send(new GetUserByTokenQuery { Token = token });
		}
	}
}