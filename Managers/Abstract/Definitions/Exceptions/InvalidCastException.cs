using System;

namespace Core.Managers.Definitions
{
	public class InvalidCastException : Exception
	{
		public InvalidCastException(string message) : base(message)
		{
		}
	}
}