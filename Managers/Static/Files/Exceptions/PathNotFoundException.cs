using System;

namespace Core.Managers.Files
{
	public class PathNotFoundException : Exception
	{
		public PathNotFoundException(string message) : base(message)
		{
		}
	}
}