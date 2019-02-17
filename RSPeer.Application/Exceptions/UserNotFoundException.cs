using System;

namespace RSPeer.Application.Exceptions
{
	public class UserNotFoundException : Exception
	{
		public UserNotFoundException(int userId)
			: base($"User was not found by Id. {userId}")
		{
		}
	}
}