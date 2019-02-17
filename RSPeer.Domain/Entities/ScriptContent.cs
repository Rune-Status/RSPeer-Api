namespace RSPeer.Domain.Entities
{
	public class ScriptContent
	{
		public int Id { get; set; }
		public Script Script { get; set; }
		public int ScriptId { get; set; }
		public byte[] Content { get; set; }
	}
}