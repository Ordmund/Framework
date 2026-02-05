using System;

namespace Core.Abstract.Exceptions
{
	public class DefinitionNotFoundException : Exception
	{
		public DefinitionNotFoundException(string message) : base(message)
		{
		}
	}
}