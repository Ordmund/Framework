using System;

namespace Framework.Managers.Definitions
{
	public class DefinitionNotFoundException : Exception
	{
		public DefinitionNotFoundException(string message) : base(message)
		{
		}
	}
}