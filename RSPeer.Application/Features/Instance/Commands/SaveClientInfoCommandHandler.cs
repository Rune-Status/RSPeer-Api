using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Instance.Commands
{
	public class SaveClientInfoCommandHandler : IRequestHandler<SaveClientInfoCommand, Unit>
	{
		private readonly RsPeerContext _db;

		public SaveClientInfoCommandHandler(RsPeerContext db)
		{
			_db = db;
		}

		public async Task<Unit> Handle(SaveClientInfoCommand request, CancellationToken cancellationToken)
		{
			request = CleanCommand(request);
			request.Client.LastUpdate = DateTimeOffset.UtcNow;

			var exists = await _db.RunescapeClients.FirstOrDefaultAsync(w =>
					w.Tag == request.Client.Tag &&
					w.RunescapeEmail == request.Client.RunescapeEmail
					&& w.ScriptName == request.Client.ScriptName
					&& w.Ip == request.Client.Ip
					&& w.ProxyIp == request.Client.ProxyIp
					&& w.OperatingSystem == request.Client.OperatingSystem,
				cancellationToken);

			if (exists != null)
			{
				exists.LastUpdate = request.Client.LastUpdate;
				exists.IsManuallyClosed = request.Client.IsManuallyClosed;
				_db.RunescapeClients.Update(exists);
			}
			else
			{
				_db.RunescapeClients.Add(request.Client);
			}


			if (request.Client.IsManuallyClosed && request.Client.Tag != Guid.Empty)
			{
				var records = await _db.RunescapeClients.Where(w => w.Tag == request.Client.Tag)
					.ToListAsync(cancellationToken);
				records.ForEach(r => r.IsManuallyClosed = true);
				_db.RunescapeClients.UpdateRange(records);
			}

			await _db.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}


		private SaveClientInfoCommand CleanCommand(SaveClientInfoCommand command)
		{
			command.Client.RunescapeEmail = command.Client.RunescapeEmail?.Trim().ToLower();
			command.Client.Rsn = command.Client.Rsn?.Trim()?.ToLower();
			command.Client.Ip = command.Client.Ip?.Trim().ToLower();
			command.Client.MachineName = command.Client.MachineName?.Trim().ToLower();
			command.Client.Id = 0;
			return command;
		}
	}
}