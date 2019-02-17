using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RSPeer.Api.Controllers.Base;
using RSPeer.Api.Middleware.Authorization;
using RSPeer.Application.Features.UserManagement.Users.Commands;
using RSPeer.Application.Features.UserManagement.Users.Models;
using RSPeer.Application.Features.UserManagement.Users.Queries;
using RSPeer.Infrastructure.Forums.Users.Commands;

namespace RSPeer.Api.Controllers
{
	public class UserController : BaseController
	{
		[HttpGet]
		[Owner]
		public async Task<ActionResult<int>> ById([FromQuery] int id, bool groups)
		{
			return Ok(await Mediator.Send(new GetUserByIdQuery
			{
				Id = id,
				IncludeGroups = groups
			}));
		}

		[HttpGet]
		public async Task<ActionResult<int>> Me()
		{
			return Ok(await GetUser());
		}

		[HttpPost]
		public async Task<ActionResult<int>> SignUp([FromBody] UserSignUpCommand command)
		{
			return Ok(await Mediator.Send(new ForumsRegisterUserCommand
			{
				Username = command.Username,
				Email = command.Email
			}));
		}

		[HttpPost]
		public async Task<ActionResult<UserSignInResult>> SignIn([FromBody] UserSignInCommand command)
		{
			return Ok(await Mediator.Send(command));
		}

		[HttpPost]
		public async Task<ActionResult<int>> UpdateBalance([FromBody] UserUpdateBalanceCommand command)
		{
			return Ok(await Mediator.Send(command));
		}

		[HttpGet]
		public async Task<ActionResult<int>> BalanceChanges([FromQuery] int userId)
		{
			return Ok(await Mediator.Send(new GetUserBalanceChangesQuery { UserId = userId }));
		}
	}
}