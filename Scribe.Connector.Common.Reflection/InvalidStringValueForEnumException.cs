using System;
using System.Runtime.Serialization;

namespace Scribe.Connector.Common.Reflection {
    public class InvalidStringValueForEnumException : Exception
    {
        public InvalidStringValueForEnumException()
        {
        }

        public InvalidStringValueForEnumException(string message) : base(message)
        {
        }

        public InvalidStringValueForEnumException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidStringValueForEnumException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}