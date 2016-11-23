using System;

namespace Ainsley.Core
{
    public class AinsleyException : Exception
    {
        public AinsleyException(string message)
        {
        }

		public AinsleyException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}