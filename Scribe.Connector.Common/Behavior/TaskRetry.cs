// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskRetry.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The task retry.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Scribe.Connector.Common.Behavior
{
    using System;
    using System.Threading.Tasks;
    using Interfaces;

    /// <summary>The task retry.</summary>
    internal static class TaskRetry
    {
        /// <summary>The retry.</summary>
        /// <param name="func">The func.</param>
        /// <param name="count">The count.</param>
        /// <param name="transientErrorDetector">The transient error detector.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public static Task<T> Retry<T>(
            Func<Task<T>> func, int count, ITransientErrorDetectionStrategy transientErrorDetector)
        {
            return RetryImpl(func, count, transientErrorDetector);
        }

        /// <summary>The retry impl.</summary>
        /// <param name="func">The func.</param>
        /// <param name="count">The count.</param>
        /// <param name="transientErrorDetector">The transient error detector.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        private static Task<T> RetryImpl<T>(
            Func<Task<T>> func, int count, ITransientErrorDetectionStrategy transientErrorDetector)
        {
            var retryTask = func().ContinueWith(
                original =>
                    {
                        count--;
                        if (original.IsFaulted)
                        {
                            if (original.Exception != null && original.Exception.InnerException != null && count > 0)
                            {
                                if (transientErrorDetector.IsTransient(original.Exception.InnerException))
                                {
                                    return RetryImpl(func, count, transientErrorDetector).Result;
                                }
                            }
                        }

                        return original.Result;
                    });

            return retryTask;
        }
    }
}