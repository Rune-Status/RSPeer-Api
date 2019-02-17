using System.Threading.Tasks;
using MediatR;
using RSPeer.Application.Features.UserManagement.Sync;

namespace MongoMigration.Actions
{
	public class MigrateUsers
	{
		private readonly IMediator _mediator;

		public MigrateUsers(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task Execute()
		{
			await _mediator.Send(new SyncUsersLocallyCommand());
		}
	}
}