using System;

namespace Core.Managers.Definitions
{
	public class DefinitionNotFoundException : Exception
	{
		public DefinitionNotFoundException(string message) : base(message)
		{
		}
	}
}