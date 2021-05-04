using System;

namespace Streaming.Core.Exceptions
{
    public class NotAvailableWebSourceException : Exception
    {
        public NotAvailableWebSourceException()
        {
        }

        public NotAvailableWebSourceException(string message)
        : base(message)
        {
        }

        public NotAvailableWebSourceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
