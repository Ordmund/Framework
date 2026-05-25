using System;

namespace Framework.Managers.Files
{
	public class FileNotFoundException : Exception
	{
		public FileNotFoundException(string message) : base(message)
		{
		}
	}
}