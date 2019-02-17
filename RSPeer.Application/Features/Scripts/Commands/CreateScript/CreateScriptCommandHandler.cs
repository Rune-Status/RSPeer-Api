using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nelibur.ObjectMapper;
using RSPeer.Application.Features.Scripts.Commands.CompileScript;
using RSPeer.Application.Features.Scripts.Queries.GetScript;
using RSPeer.Domain.Entities;
using RSPeer.Persistence;

namespace RSPeer.Application.Features.Scripts.Commands.CreateScript
{
	public class CreateScriptCommandHandler : IRequestHandler<CreateScriptCommand, int>
	{
		private readonly RsPeerContext _db;
		private readonly IMediator _mediator;

		public CreateScriptCommandHandler(IMediator mediator, RsPeerContext db)
		{
			_mediator = mediator;
			_db = db;
		}

		public async Task<int> Handle(CreateScriptCommand request, CancellationToken cancellationToken)
		{
			request = CleanCommand(request);
			using (var transaction = await _db.Database.BeginTransactionAsync(cancellationToken))
			{
				var script = request.Script;

				var exists =
					await _db.Scripts.FirstOrDefaultAsync(s =>
						s.Name.ToLower() == script.Name.ToLower() || s.Id == script.Id, cancellationToken);

				await AssertCanCreate(request, exists);

				var isUpdatingPending =
					exists != null && exists.Status == ScriptStatus.Pending && script.Id != default(int);

				script.UserId = request.User.Id;
				script.Status = ScriptStatus.Pending;
				script.LastUpdate = DateTimeOffset.UtcNow;
				script.Version = exists?.Version + (decimal) 0.01 ?? (decimal) 0.01;

				if (isUpdatingPending)
				{
					TinyMapper.Map(script, exists);
					_db.Scripts.Update(exists);
				}
				else
				{
					script.Id = 0;
					await _db.Scripts.AddAsync(script, cancellationToken);
				}

				await _db.SaveChangesAsync(cancellationToken);

				if (exists != null && !isUpdatingPending)
					await _db.PendingScripts.AddAsync(new PendingScript
					{
						LiveScriptId = exists.Id,
						PendingScriptId = script.Id
					}, cancellationToken);

				var compile =
					await _mediator.Send(
						new CompileScriptCommand { GitlabUrl = request.RepositoryUrl },
						cancellationToken);

				if (isUpdatingPending)
				{
					var content = await _mediator.Send(new GetScriptContentQuery { ScriptId = exists.Id },
						cancellationToken);
					content.Content = compile.Content;
					_db.Update(content);
				}
				else
				{
					await _db.ScriptContents.AddAsync(new ScriptContent
					{
						ScriptId = script.Id,
						Content = compile.Content
					}, cancellationToken);
				}

				await _db.SaveChangesAsync(cancellationToken);

				transaction.Commit();

				return isUpdatingPending ? exists.Id : script.Id;
			}
		}

		private CreateScriptCommand CleanCommand(CreateScriptCommand command)
		{
			command.Script.Name = command.Script.Name.Trim();
			command.Script.Description = command.Script.Description.Trim();
			command.Script.TotalUsers = 0;
			command.Script.ScriptContent = null;
			return command;
		}

		private async Task AssertCanCreate(CreateScriptCommand request, Script current)
		{
			if (current != null && current.UserId != request.User.Id)
				throw new Exception("Script already exists by that name.");

			if (current != null && request.Script.Id != current.Id)
				throw new Exception(
					"You already have a script by this name, did you mean to update that script instead?");
		}
	}
}