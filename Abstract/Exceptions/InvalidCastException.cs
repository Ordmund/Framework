using System;

namespace Core.Abstract.Exceptions
{
	public class InvalidCastException : Exception
	{
		public InvalidCastException(string message) : base(message)
		{
		}
	}
}