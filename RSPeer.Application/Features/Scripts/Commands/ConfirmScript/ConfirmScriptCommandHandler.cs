using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nelibur.ObjectMapper;
using RSPeer.Application.Exceptions;
using RSPeer.Application.Features.Scripts.Queries.GetScript;
using RSPeer.Application.Features.Store.Items.Commands;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Scripts.Commands.ConfirmScript
{
	public class ConfirmScriptCommandHandler : IRequestHandler<ConfirmScriptCommand, Unit>
	{
		private readonly RsPeerContext _db;
		private readonly IMediator _mediator;

		public ConfirmScriptCommandHandler(RsPeerContext db, IMediator mediator)
		{
			_db = db;
			_mediator = mediator;
		}

		public async Task<Unit> Handle(ConfirmScriptCommand request, CancellationToken cancellationToken)
		{
			var pendingScript =
				await _db.Scripts.FirstOrDefaultAsync(s =>
					s.Id == request.ScriptId && s.Status == ScriptStatus.Pending, cancellationToken);

			if (pendingScript == null) throw new NotFoundException("PendingScript", request.ScriptId);

			var map = await _db.PendingScripts.FirstOrDefaultAsync(w => w.PendingScriptId == pendingScript.Id,
				cancellationToken);

			var script =
				await _mediator.Send(new GetScriptByIdQuery { ScriptId = map.LiveScriptId }, cancellationToken);

			TinyMapper.Map(pendingScript, script);

			script.Status = ScriptStatus.Live;

			var oldContent =
				await _db.ScriptContents.FirstOrDefaultAsync(w => w.ScriptId == script.Id, cancellationToken);
			var newContent =
				await _db.ScriptContents.FirstOrDefaultAsync(w => w.ScriptId == pendingScript.Id, cancellationToken);

			newContent.ScriptId = script.Id;

			_db.Scripts.Remove(pendingScript);
			_db.Scripts.Update(script);
			_db.ScriptContents.Update(newContent);
			_db.ScriptContents.Remove(oldContent);
			_db.PendingScripts.Remove(map);

			if (script.Price.HasValue && script.Type == ScriptType.Premium)
				await _mediator.Send(new AddItemCommand
				{
					Upsert = true,
					Description = script.Description,
					Name = script.Name,
					PaymentMethod = PaymentMethod.Tokens,
					Price = script.Price.Value,
					Sku = $"premium-script-{script.Id}"
				}, cancellationToken);

			await _db.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}