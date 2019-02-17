using System;

namespace RSPeer.Application.Exceptions
{
	public class PaypalException : Exception
	{
		public PaypalException(string message) : base(message)
		{
		}
	}
}