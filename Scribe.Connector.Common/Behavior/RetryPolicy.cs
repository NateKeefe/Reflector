// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RetryPolicy.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The retry policy.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Behavior
{
    using System;
    using System.Threading;

    /// <summary>The retry policy.</summary>
    public static class RetryPolicy
    {
        /// <summary>The execute action.</summary>
        /// <param name="func">The func.</param>
        /// <param name="shouldRetry">The should retry.</param>
        /// <param name="isTransient">The is transient.</param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns>The <see cref="TResult"/>.</returns>
        public static TResult ExecuteAction<TResult>(
            Func<TResult> func, ShouldRetry shouldRetry, Func<Exception, bool> isTransient)
        {
            return ExecuteAction(func, shouldRetry, isTransient, true, null);
        }

        /// <summary>The execute action.</summary>
        /// <param name="func">The func.</param>
        /// <param name="shouldRetry">The should retry.</param>
        /// <param name="isTransient">The is transient.</param>
        /// <param name="retryFirstFast">The retry first fast.</param>
        /// <param name="onRetrying">The on retrying.</param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns>The <see cref="TResult"/>.</returns>
        public static TResult ExecuteAction<TResult>(
            Func<TResult> func, 
            ShouldRetry shouldRetry, 
            Func<Exception, bool> isTransient, 
            bool retryFirstFast, 
            Action<int, Exception, TimeSpan> onRetrying)
        {
            // Contract.Requires<ArgumentNullException>(func != null, "Cannot execute a null func in a retry context.");
            var retryCount = 0;
            while (true)
            {
                TimeSpan delay;
                do
                {
                    try
                    {
                        return func();
                    }
                    catch (Exception ex)
                    {
                        if (!isTransient(ex) || !shouldRetry(retryCount++, ex, out delay))
                        {
                            throw;
                        }

                        if (delay.TotalMilliseconds < 0.0)
                        {
                            delay = TimeSpan.Zero;
                        }

                        if (onRetrying != null)
                        {
                            onRetrying(retryCount, ex, delay);
                        }
                    }
                }
                while (retryCount <= 1 && retryFirstFast);
                Thread.Sleep(delay);
            }
        }

        /// <summary>The execute action.</summary>
        /// <param name="beginAction">The begin action.</param>
        /// <param name="endAction">The end action.</param>
        /// <param name="successHandler">The success handler.</param>
        /// <param name="faultHandler">The fault handler.</param>
        /// <param name="shouldRetry">The should retry.</param>
        /// <param name="isTransient">The is transient.</param>
        /// <param name="onRetrying">The on retrying.</param>
        /// <param name="fastFirstRetry">The fast first retry.</param>
        /// <typeparam name="TResult"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ExecuteAction<TResult>(
            Action<AsyncCallback> beginAction, 
            Func<IAsyncResult, TResult> endAction, 
            Action<TResult> successHandler, 
            Action<Exception> faultHandler, 
            ShouldRetry shouldRetry, 
            Func<Exception, bool> isTransient, 
            Action<int, Exception, TimeSpan> onRetrying, 
            bool fastFirstRetry)
        {
            if (beginAction == null)
            {
                throw new ArgumentNullException("beginAction", "Start action must not be null");
            }

            if (endAction == null)
            {
                throw new ArgumentNullException("endAction", "End action must not be null");
            }

            // if the handlers are not specified create default empty ones.
            successHandler = successHandler ?? (_ => { });
            faultHandler = faultHandler ?? (_ => { });

            var retryCount = 0;
            AsyncCallback endInvoke = null;
            Func<Action, bool> executeWithRetry = null;

            // Wrap the result in something that knows how to retry
            endInvoke = ar =>
                {
                    var result = default(TResult);

                    if (executeWithRetry(() => result = endAction(ar)))
                    {
                        successHandler(result);
                    }
                };

            // Actual retry logic
            executeWithRetry = a =>
                {
                    try
                    {
                        // Invoke the callback delegate which can throw an exception if the main async operation has completed with a fault.
                        a();

                        // Get out of the loop
                        return true;
                    }
                    catch (Exception ex)
                    {
                        // Capture the original exception for analysis.
                        var lastError = ex;

                        TimeSpan delay;

                        // Check if we should continue retrying on this exception. If not, invoke the fault handler so that user code can take control.
                        if (!(isTransient(lastError) && shouldRetry(retryCount++, lastError, out delay)))
                        {
                            // Notify of failure
                            faultHandler(lastError);
                        }
                        else
                        {
                            // Notify the respective subscribers about this exception.
                            onRetrying(retryCount, lastError, delay);

                            // Sleep for the defined interval before repetitively executing the main async operation.
                            if (retryCount > 1 || !fastFirstRetry)
                            {
                                Thread.Sleep(delay);
                            }

                            // Retry
                            executeWithRetry(() => beginAction(endInvoke));
                        }

                        // indicate this was not successful (either retrying or done)
                        return false;
                    }
                };

            // Invoke the the main async operation for the first time which should return control to the caller immediately.
            executeWithRetry(() => beginAction(endInvoke));
        }
    }
}