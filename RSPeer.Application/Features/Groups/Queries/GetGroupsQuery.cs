using System.Collections.Generic;
using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Groups.Queries
{
	public class GetGroupsQuery : IRequest<IEnumerable<Group>>
	{
	}
}