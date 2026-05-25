using System;

namespace Framework.Managers.Files
{
	public class PathNotFoundException : Exception
	{
		public PathNotFoundException(string message) : base(message)
		{
		}
	}
}