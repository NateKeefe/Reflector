// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordNotFoundException.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2013 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Exceptions
{
    /// <summary>
    /// This exception occurs when attempting an operation on a record that does not exist.
    /// </summary>
    public class RecordNotFoundException : System.ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordNotFoundException"/> class.
        /// </summary>
        public RecordNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordNotFoundException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public RecordNotFoundException(string message) : base(message)
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordNotFoundException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public RecordNotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
