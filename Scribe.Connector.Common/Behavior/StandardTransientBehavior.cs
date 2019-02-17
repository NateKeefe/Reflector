// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardTransientBehavior.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   SOAP based web service transient behavior.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Behavior
{
    using System;
    using System.Net;
    using System.Web.Services.Protocols;

    /// <summary>
    ///   SOAP based web service transient behavior.
    /// </summary>
    public static class StandardTransientBehavior
    {
        /// <summary>Determines whether an exception is transient.</summary>
        /// <param name="ex">The exception. </param>
        /// <returns>True if the exception is determined to be transient, otherwise false. </returns>
        public static bool IsTransient(Exception ex)
        {
            return ex is SoapException || ex is WebException || ex is TimeoutException;
        }

        /// <summary>Simple implementation of a ShouldRetry.</summary>
        /// <param name="count">The number of times this has been retried. </param>
        /// <param name="ex">The exception. </param>
        /// <param name="delay">Sets the amount of time to delay before retrying. </param>
        /// <returns>The System.Boolean. </returns>
        /// <remarks>The delay increases as the number of retries does.</remarks>
        public static bool ShouldRetryStandard(int count, Exception ex, out TimeSpan delay)
        {
            delay = TimeSpan.Zero;
            if (count > 3)
            {
                return false;
            }

            delay = TimeSpan.FromMilliseconds(100 * count);

            return true;
        }
    }
}