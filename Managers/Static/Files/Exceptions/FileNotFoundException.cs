using System;

namespace Core.Managers.Files
{
    public class FileNotFoundException : Exception
    {
        public FileNotFoundException(string message) : base(message)
        {
        }
    }
}