using System;

namespace Core.Managers.Behaviours
{
	public class FactoryInitializationException : Exception
	{
		public FactoryInitializationException(string message) : base(message)
		{
		}
	}
}