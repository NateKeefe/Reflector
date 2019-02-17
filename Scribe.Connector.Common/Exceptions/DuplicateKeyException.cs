// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DuplicateKeyException.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2013 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Scribe.Connector.Common.Exceptions
{
    /// <summary>This exception occurs when attempting an operation which violated a unique index constraint.</summary>
    public class DuplicateKeyException : ApplicationException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DuplicateKeyException" /> class.
        /// </summary>
        public DuplicateKeyException() { }

        /// <summary>Initializes a new instance of the <see cref="DuplicateKeyException" /> class.</summary>
        /// <param name="message">The message.</param>
        public DuplicateKeyException(string message)
            : base(message) { }

        /// <summary>Initializes a new instance of the <see cref="DuplicateKeyException" /> class.</summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DuplicateKeyException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}