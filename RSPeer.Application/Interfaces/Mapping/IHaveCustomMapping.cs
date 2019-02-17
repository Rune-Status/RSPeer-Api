using AutoMapper;

namespace RSPeer.Application.Interfaces.Mapping
{
	public interface IHaveCustomMapping
	{
		void CreateMappings(Profile configuration);
	}
}