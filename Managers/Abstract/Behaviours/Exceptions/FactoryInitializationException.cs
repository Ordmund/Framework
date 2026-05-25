using System;

namespace Framework.Managers.Behaviours
{
	public class FactoryInitializationException : Exception
	{
		public FactoryInitializationException(string message) : base(message)
		{
		}
	}
}