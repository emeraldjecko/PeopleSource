using System;
using System.Runtime.Serialization;

namespace PeoplesSource
{
    [Serializable]
    public class HetsException : Exception
    {

        public HetsException()
        { }

        public HetsException(string message)
            : base(message) { }

        public HetsException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public HetsException(string message, Exception innerException)
            : base(message, innerException) { }

        public HetsException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected HetsException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

    }
}