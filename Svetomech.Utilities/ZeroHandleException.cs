using System;
using System.Runtime.Serialization;

namespace Svetomech.Utilities
{
    [Serializable]
    internal class ZeroHandleException : Exception
    {
        public ZeroHandleException()
        {
        }

        public ZeroHandleException(string message) : base(message)
        {
        }

        public ZeroHandleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ZeroHandleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}