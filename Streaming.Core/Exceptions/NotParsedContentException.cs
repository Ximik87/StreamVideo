using System;

namespace Streaming.Core.Exceptions
{
    public class NotParsedContentException : Exception
    {
        public NotParsedContentException()
        {
        }

        public NotParsedContentException(string message)
        : base(message)
        {
        }

        public NotParsedContentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
