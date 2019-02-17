using System.Collections.Generic;
using MediatR;
using RSPeer.Domain.Entities;

namespace RSPeer.Application.Features.Store.Items.Queries
{
	public class GetItemsQuery : IRequest<IEnumerable<Item>>
	{
	}
}