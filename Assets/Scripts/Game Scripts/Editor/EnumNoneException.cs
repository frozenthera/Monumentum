using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum.Exceptions
{
    [Serializable]
    public class EnumNoneException : Exception
    {
        public EnumNoneException()
        {
        }

        public EnumNoneException(string message) : base(message)
        {
        }

        public EnumNoneException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EnumNoneException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
    }
}