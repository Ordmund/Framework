using System;

namespace Framework.Managers.Definitions
{
	public class InvalidCastException : Exception
	{
		public InvalidCastException(string message) : base(message)
		{
		}
	}
}