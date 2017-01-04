using System;

namespace Remy.Core
{
    public class RemyException : Exception
    {
        public RemyException(string message) : base(message)
        {
        }

		public RemyException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}