using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Groups.Commands
{
	public class AddGroupCommandHandler : IRequestHandler<AddGroupCommand, int>
	{
		private readonly RsPeerContext _db;

		public AddGroupCommandHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<int> Handle(AddGroupCommand request, CancellationToken cancellationToken)
		{
			var group = new Group
			{
				Name = request.Name,
				Description = request.Description
			};
			_db.Groups.Add(group);
			await _db.SaveChangesAsync(cancellationToken);
			return group.Id;
		}
	}
}