using System;

namespace Core.Managers.Files
{
    public class TypeIsNotSerializableException : Exception
    {
        public TypeIsNotSerializableException(string message) : base(message)
        {
        }
    }
}