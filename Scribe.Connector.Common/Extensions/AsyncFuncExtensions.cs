// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AsyncFuncExtensions.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   Extensions for asynchronous functions (Functions that return a Task).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Scribe.Connector.Common.Behavior;

namespace Scribe.Connector.Common.Extensions
{
    using System;
    using System.Threading.Tasks;
    using Interfaces;

    /// <summary>
    ///   Extensions for asynchronous functions (Functions that return a Task).
    /// </summary>
    internal static class AsyncFuncExtensions
    {
        /// <summary>Retries an asynchronous function.</summary>
        /// <param name="func">The function that should be retried. </param>
        /// <param name="count">The number of times to retry. </param>
        /// <param name="transientErrorDetector">The strategy for detecting which exceptions are transient. </param>
        /// <typeparam name="T">The result of the Task. </typeparam>
        /// <returns>The <see cref="Task"/> . </returns>
        public static Task<T> Retry<T>(
            this Func<Task<T>> func, int count, ITransientErrorDetectionStrategy transientErrorDetector)
        {
            return TaskRetry.Retry(func, count, transientErrorDetector);
        }

        /// <summary>Retries an asynchronous function.</summary>
        /// <param name="func">The function that should be retried. </param>
        /// <param name="count">The number of times to retry. </param>
        /// <param name="transientErrorDetectorFunction">The strategy for detecting which exceptions are transient. </param>
        /// <typeparam name="T">The result of the Task. </typeparam>
        /// <returns>The <see cref="Task"/> . </returns>
        public static Task<T> Retry<T>(
            this Func<Task<T>> func, int count, Func<Exception, bool> transientErrorDetectorFunction)
        {
            var transientErrorDetector = new TransientErrorDetector(transientErrorDetectorFunction);
            return TaskRetry.Retry(func, count, transientErrorDetector);
        }

        /// <summary>Adds retry to the function.</summary>
        /// <param name="func">The function to retry. </param>
        /// <param name="count">The number of times to retry. </param>
        /// <param name="transientErrorDetector">The strategy for detecting which exceptions are transient. </param>
        /// <typeparam name="T">The result of the Task. </typeparam>
        /// <returns>The <see cref="Func{TResult}"/> . </returns>
        public static Func<Task<T>> WithRetry<T>(
            this Func<Task<T>> func, int count, ITransientErrorDetectionStrategy transientErrorDetector)
        {
            return () => TaskRetry.Retry(func, count, transientErrorDetector);
        }

        /// <summary>Adds retry to the function.</summary>
        /// <param name="func">The function to retry. </param>
        /// <param name="count">The number of times to retry. </param>
        /// <param name="transientErrorDetectorFunction">The strategy for detecting which exceptions are transient. </param>
        /// <typeparam name="T">The result of the Task. </typeparam>
        /// <returns>The <see cref="Func{TResult}"/> . </returns>
        public static Func<Task<T>> WithRetry<T>(
            this Func<Task<T>> func, int count, Func<Exception, bool> transientErrorDetectorFunction)
        {
            var transientErrorDetector = new TransientErrorDetector(transientErrorDetectorFunction);
            return () => TaskRetry.Retry(func, count, transientErrorDetector);
        }
    }
}