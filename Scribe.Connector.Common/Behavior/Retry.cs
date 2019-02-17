// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Retry.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The retry.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Behavior
{
    using System;

    /// <summary>The retry.</summary>
    public static class Retry
    {
        /// <summary>The execute action.</summary>
        /// <param name="func">The func.</param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns>The <see cref="TResult"/>.</returns>
        public static TResult ExecuteAction<TResult>(Func<TResult> func)
        {
            ShouldRetry shouldRetry = StandardTransientBehavior.ShouldRetryStandard;
            Func<Exception, bool> detectionStrategy = StandardTransientBehavior.IsTransient;
            return RetryPolicy.ExecuteAction(func, shouldRetry, detectionStrategy);
        }
    }
}