using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.UserManagement.Users.Queries
{
	public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
	{
		private readonly RsPeerContext _db;

		public GetUserByIdQueryHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
		{
			var queryable = _db.Users.AsQueryable().Where(w => w.Id == request.Id);
			if (request.IncludeGroups)
				queryable = queryable.Include(w => w.UserGroups)
					.ThenInclude(w => w.Group);
			return await queryable.FirstOrDefaultAsync(cancellationToken);
		}
	}
}