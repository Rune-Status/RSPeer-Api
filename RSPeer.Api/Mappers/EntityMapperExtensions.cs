using Nelibur.ObjectMapper;
using RSPeer.Domain.Entities;

namespace RSPeer.Api.Mappers
{
	public static class EntityMapperExtensions
	{
		public static void AddEntityMappers()
		{
			TinyMapper.Bind<Script, Script>(config =>
			{
				config.Ignore(w => w.Id);
				config.Ignore(w => w.TotalUsers);
				config.Ignore(w => w.ScriptContent);
			});

			TinyMapper.Bind<Item, Item>(config =>
			{
				config.Ignore(w => w.Sku);
				config.Ignore(w => w.Id);
			});
		}
	}
}