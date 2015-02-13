using System;
using SimpleCqrs.Domain;
using SimpleCQRSDemo.Events;
using System.Runtime.Serialization;

namespace SimpleCQRSDemo.Domain
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class NsfException : Exception
    {
        // constructors...
        #region NsfException()
        /// <summary>
        /// Constructs a new NsfException.
        /// </summary>
        public NsfException() { }
        #endregion
        #region NsfException(string message)
        /// <summary>
        /// Constructs a new NsfException.
        /// </summary>
        /// <param name="message">The exception message</param>
        public NsfException(string message) : base(message) { }
        #endregion
        #region NsfException(string message, Exception innerException)
        /// <summary>
        /// Constructs a new NsfException.
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerException">The inner exception</param>
        public NsfException(string message, Exception innerException) : base(message, innerException) { }
        #endregion
        #region NsfException(SerializationInfo info, StreamingContext context)
        /// <summary>
        /// Serialization constructor.
        /// </summary>
        protected NsfException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        #endregion
    }
}
