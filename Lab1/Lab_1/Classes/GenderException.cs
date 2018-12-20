using System;
using System.Runtime.Serialization;

namespace Lab_1.Classes
{
    public class GenderException : Exception
    {
        public GenderException(string message) : base(message)
        {
        }

        public GenderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GenderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
