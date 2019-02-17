// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransientErrorDetector.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The transient error detector.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Behavior
{
    using System;

    using Scribe.Connector.Common.Interfaces;

    /// <summary>
    ///   The transient error detector.
    /// </summary>
    internal class TransientErrorDetector : ITransientErrorDetectionStrategy
    {
        /// <summary>
        ///   The transient error detector.
        /// </summary>
        private readonly Func<Exception, bool> transientErrorDetector;

        /// <summary>Initializes a new instance of the <see cref="TransientErrorDetector"/> class.</summary>
        /// <param name="transientErrorDetector">The transient error detector. </param>
        public TransientErrorDetector(Func<Exception, bool> transientErrorDetector)
        {
            this.transientErrorDetector = transientErrorDetector;
        }

        /// <summary>The is transient.</summary>
        /// <param name="exception">The exception. </param>
        /// <returns>The <see cref="bool"/> . </returns>
        public bool IsTransient(Exception exception)
        {
            return this.transientErrorDetector(exception);
        }
    }
}