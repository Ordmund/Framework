using System;

namespace Framework.Managers.Files
{
	public class TypeIsNotSerializableException : Exception
	{
		public TypeIsNotSerializableException(string message) : base(message)
		{
		}
	}
}