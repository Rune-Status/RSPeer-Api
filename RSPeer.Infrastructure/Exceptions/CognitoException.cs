using System;

namespace RSPeer.Infrastructure.Exceptions
{
	public class CognitoException : Exception
	{
		public CognitoException(string message) : base(message)
		{
		}
	}
}