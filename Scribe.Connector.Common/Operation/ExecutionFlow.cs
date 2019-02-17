// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecutionFlow.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The execution flow that converts input feeds that to an e.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Scribe.Connector.Common.Interfaces;
using Scribe.Connector.Common.Behavior;
using Scribe.Core.ConnectorApi.Exceptions;
using Scribe.Core.ConnectorApi.Logger;

namespace Scribe.Connector.Common.Operation
{
    /// <summary>
    ///   The execution flow that converts input feeds that to an e.
    /// </summary>
    public static class ExecutionFlow
    {
        /// <summary>The batch size.</summary>
        private const int DefaultBatchSize = 100;

        /// <summary>Execute the process.</summary>
        /// <param name="input">The input. </param>
        /// <param name="inputConverter">The input converter. </param>
        /// <param name="execute">The execute method. </param>
        /// <param name="resultConverter">The result converter. </param>
        /// <typeparam name="TIn">The type of the input. </typeparam>
        /// <typeparam name="TOut">The type of the value on the IIndexedResult of the final result. </typeparam>
        /// <returns>The returned results. </returns>
        public static IList<ResultItem> Execute<TIn, TOut>(
            IList<InputItem> input, 
            Func<InputItem, TIn> inputConverter, 
            Func<IList<TIn>, IList<TOut>> execute, 
            Func<TOut, ResultItem> resultConverter)
        {
            var lengthOfInput = input.Count;

            var indexedInputItems = GenerateIndexedInputItems(input);

            IList<IIndexed<ResultItem>> unsuccessfullyConvertedInput;

            var successfullyConvertedInput = ProcessInputConversions(
                inputConverter, indexedInputItems, out unsuccessfullyConvertedInput);

            LogResultsOfInputConversion(lengthOfInput, successfullyConvertedInput, unsuccessfullyConvertedInput);

            var chunks = BreakIntoChunks(successfullyConvertedInput.ToList(), DefaultBatchSize);

            var executionResults = new List<IIndexed<ResultItem>>();

            foreach (var chunk in chunks)
            {
                executionResults.AddRange(ExecuteInternal(execute, resultConverter, chunk));
            }

            var finalResults = unsuccessfullyConvertedInput.Concat(executionResults).OrderBy(i => i.Index);

            // Log the whole thing.
            return finalResults.Select(i => i.Value).ToList();
        }

        /// <summary>Execute the process.</summary>
        /// <param name="input">The input. </param>
        /// <param name="inputConverter">The input converter. </param>
        /// <param name="execute">The execute method. </param>
        /// <param name="resultConverter">The result converter. </param>
        /// <param name="batchSize"></param>
        /// <typeparam name="TIn">The type of the input. </typeparam>
        /// <typeparam name="TOut">The type of the value on the IIndexedResult of the final result. </typeparam>
        /// <returns>The returned results. </returns>
        public static IList<ResultItem> Execute<TIn, TOut>(
            IList<InputItem> input,
            Func<InputItem, TIn> inputConverter,
            Func<IList<TIn>, IList<TOut>> execute,
            Func<TOut, ResultItem> resultConverter,
            int batchSize)
        {
            var lengthOfInput = input.Count;

            var indexedInputItems = GenerateIndexedInputItems(input);

            IList<IIndexed<ResultItem>> unsuccessfullyConvertedInput;

            var successfullyConvertedInput = ProcessInputConversions(
                inputConverter, indexedInputItems, out unsuccessfullyConvertedInput);

            LogResultsOfInputConversion(lengthOfInput, successfullyConvertedInput, unsuccessfullyConvertedInput);

            var chunks = BreakIntoChunks(successfullyConvertedInput.ToList(), batchSize);

            var executionResults = new List<IIndexed<ResultItem>>();

            foreach (var chunk in chunks)
            {
                executionResults.AddRange(ExecuteInternalBasic(execute, resultConverter, chunk));
            }

            var finalResults = unsuccessfullyConvertedInput.Concat(executionResults).OrderBy(i => i.Index);

            // Log the whole thing.
            return finalResults.Select(i => i.Value).ToList();
        }

        public static IList<ResultItem> ExecuteAsync<TIn, TOut>(
            IList<InputItem> input,
            Func<InputItem, TIn> inputConverter,
            Func<IList<TIn>, Task<IList<TOut>>> execute,
            Func<TOut, ResultItem> resultConverter,
            int batchSize)
        {
            Debug.WriteLine("Converting input");

            var lengthOfInput = input.Count;

            var indexedInputItems = GenerateIndexedInputItems(input);

            IList<IIndexed<ResultItem>> unsuccessfullyConvertedInput;

            var successfullyConvertedInput = ProcessInputConversions(
                inputConverter, indexedInputItems, out unsuccessfullyConvertedInput);

            LogResultsOfInputConversion(lengthOfInput, successfullyConvertedInput, unsuccessfullyConvertedInput);

            var chunks = BreakIntoChunks(successfullyConvertedInput.ToList(), batchSize);

            var tasks = new Task<List<IIndexed<ResultItem>>>[chunks.Count];

            var index = 0;
            foreach (var chunk in chunks)
            {
                var unIndexedChunk = chunk.Select(i => i.Value).ToList();

                // Execute the operation using the set of unindexed values from the chunk.
                // Results should always be returned in the same order that they were received.
                // If the execution fails perform retry logic.
                // Process the results after completion turning them into indexed result items.
                // Exceptions cause each indexed result item to contain the identical error information from the exception.
                var chunk1 = chunk;
                var tsk =
                    TaskRetry.Retry(
                        Curry(execute, unIndexedChunk),
                        3,
                        new TransientErrorDetector(StandardTransientBehavior.IsTransient)).ContinueWith(
                            t => ProcessResults(t, resultConverter, chunk1));

                tasks[index] = tsk;

                index++;
            }

            WaitForTasksToComplete(tasks);

            var executionResults = new List<IIndexed<ResultItem>>();

            foreach (var task in tasks)
            {
                executionResults.AddRange(task.Result);
            }

            var mergedResults = unsuccessfullyConvertedInput.Concat(executionResults).OrderBy(i => i.Index);

            var finalResults = mergedResults.Select(i => i.Value).ToList();

            ValidateFinalResults(finalResults);

            // Log the whole thing.
            return finalResults;
        }

        /// <summary>The execute async.</summary>
        /// <param name="input">The input.</param>
        /// <param name="inputConverter">The input converter.</param>
        /// <param name="execute">The execute.</param>
        /// <param name="resultConverter">The result converter.</param>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <returns>The <see cref="IList"/>.</returns>
        public static IList<ResultItem> ExecuteAsync<TIn, TOut>(
            IList<InputItem> input, 
            Func<InputItem, TIn> inputConverter, 
            Func<IList<TIn>, Task<IList<TOut>>> execute, 
            Func<TOut, ResultItem> resultConverter)
        {
            return ExecuteAsync(input, inputConverter, execute, resultConverter, DefaultBatchSize);
        }

        public static IList<ResultItem> ExecuteAsync<TIn, TOut>(
            IList<InputItem> input,
            Func<InputItem, TIn> inputConverter,
            Func<TIn, Task<TOut>> execute,
            Func<TOut, ResultItem> resultConverter)
        {
            Debug.WriteLine("Converting input");

            var lengthOfInput = input.Count;

            var indexedInputItems = GenerateIndexedInputItems(input);

            IList<IIndexed<ResultItem>> unsuccessfullyConvertedInput;

            var successfullyConvertedInput = ProcessInputConversions(inputConverter, indexedInputItems, out unsuccessfullyConvertedInput);

            LogResultsOfInputConversion(lengthOfInput, successfullyConvertedInput, unsuccessfullyConvertedInput);

            var tasks = new Task<IIndexed<ResultItem>>[successfullyConvertedInput.Count];
            int index = 0;
            foreach (var inputItem in successfullyConvertedInput)
            {
                // Execute the operation using the set of unindexed values from the chunk.
                // Results should always be returned in the same order that they were received.
                // If the execution fails perform retry logic.
                // Process the results after completion turning them into indexed result items.
                // Exceptions cause each indexed result item to contain the identical error information from the exception.
                var tsk = TaskRetry.Retry(Curry(execute, inputItem.Value), 3, new TransientErrorDetector(StandardTransientBehavior.IsTransient))
                    .ContinueWith(t => ConvertResultInternal(resultConverter, inputItem, t.Result));

                tasks[index] = tsk;

                index++;
            }

            WaitForTasksToComplete(tasks);

            var executionResults = tasks.Select(task => task.Result).ToList();

            var mergedResults = unsuccessfullyConvertedInput.Concat(executionResults).OrderBy(i => i.Index);

            var finalResults = mergedResults.Select(i => i.Value).ToList();

            ValidateFinalResults(finalResults);

            // Log the whole thing.
            return finalResults;
        }


        private static IIndexed<ResultItem> ConvertResultInternal<T2, T3>(Func<T3, ResultItem> resultConverter, IIndexed<T2> input, T3 result)
        {
            var convertedRes = resultConverter(result);

            return new Indexed<ResultItem>(convertedRes, input.Index);
        }

        /// <summary>The break into chunks.</summary>
        /// <param name="list">The list.</param>
        /// <param name="batchSize">the number of elements in a chunk</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>the list of chunks</returns>
        private static List<List<T>> BreakIntoChunks<T>(List<T> list, int batchSize)
        {
            var retVal = new List<List<T>>();
            var orgCount = list.Count;

            while (list.Count > 0)
            {
                var count = list.Count > batchSize ? batchSize : list.Count;
                retVal.Add(list.GetRange(0, count));
                list.RemoveRange(0, count);
            }

            var msg = string.Format("{0} records were broken into {1} chunks.", orgCount, retVal.Count);

            Debug.WriteLine(msg);
            Logger.Write(Logger.Severity.Debug, "Chunking input values completed.", msg);

            return retVal;
        }

        /// <summary>The convert results internal.</summary>
        /// <param name="resultConverter">The result converter.</param>
        /// <param name="indexedInputs">The indexed inputs.</param>
        /// <param name="results">The results.</param>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <returns>The <see cref="IList"/>.</returns>
        private static IList<IIndexed<ResultItem>> ConvertResultsInternal<T2, T3>(
            Func<T3, ResultItem> resultConverter, IList<IIndexed<T2>> indexedInputs, IList<T3> results)
        {
            var indexedResults = new List<IIndexed<ResultItem>>();

            var index = 0;
            foreach (var result in results)
            {
                var convertedRes = resultConverter(result);
                var indexedInput = indexedInputs[index];

                var indexedItem = new Indexed<ResultItem>(convertedRes, indexedInput.Index);

                indexedResults.Add(indexedItem);
                index++;
            }

            return indexedResults;
        }

        /// <summary>The curry.</summary>
        /// <param name="execute">The execute.</param>
        /// <param name="input">The input.</param>
        /// <typeparam name="TArg1"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns>The <see cref="Func"/>.</returns>
        private static Func<TResult> Curry<TArg1, TResult>(Func<TArg1, TResult> execute, TArg1 input)
        {
            return () => execute(input);
        }

        /// <summary>The execute internal.</summary>
        /// <param name="execute">The execute.</param>
        /// <param name="resultConverter">The result converter.</param>
        /// <param name="inputs">The inputs.</param>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <returns>The <see cref="IList"/>.</returns>
        private static IList<IIndexed<ResultItem>> ExecuteInternal<TIn, TOut>(
            Func<IList<TIn>, IList<TOut>> execute, Func<TOut, ResultItem> resultConverter, IList<IIndexed<TIn>> inputs)
        {
            var executionResults = new List<IIndexed<ResultItem>>();

            var unIndexedChunk = inputs.Select(i => i.Value).ToList();

            try
            {
                var results = Retry.ExecuteAction(Curry(execute, unIndexedChunk));
                executionResults.AddRange(ConvertResultsInternal(resultConverter, inputs, results));
            }
            catch (Exception e)
            {
                var errorResultItem = new ResultItem(0, true, e);
                var indexedErrors = inputs.Select(i => new Indexed<ResultItem>(errorResultItem, i.Index));
                executionResults.AddRange(indexedErrors);
            }

            return executionResults;
        }

        /// <summary>The execute internal.</summary>
        /// <param name="execute">The execute.</param>
        /// <param name="resultConverter">The result converter.</param>
        /// <param name="inputs">The inputs.</param>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <returns>The <see cref="IList"/>.</returns>
        private static IList<IIndexed<ResultItem>> ExecuteInternalBasic<TIn, TOut>(
            Func<IList<TIn>, IList<TOut>> execute, Func<TOut, ResultItem> resultConverter, IList<IIndexed<TIn>> inputs)
        {
            var executionResults = new List<IIndexed<ResultItem>>();

            var unIndexedChunk = inputs.Select(i => i.Value).ToList();

            try
            {
                var results = execute(unIndexedChunk);
                executionResults.AddRange(ConvertResultsInternal(resultConverter, inputs, results));
            }
            catch (Exception e)
            {
                var errorResultItem = new ResultItem(0, true, e);
                var indexedErrors = inputs.Select(i => new Indexed<ResultItem>(errorResultItem, i.Index));
                executionResults.AddRange(indexedErrors);
            }

            return executionResults;
        }

        /// <summary>The generate indexed input items.</summary>
        /// <param name="inputItems">The input items.</param>
        /// <returns>The <see cref="IList"/>.</returns>
        private static IList<IIndexed<InputItem>> GenerateIndexedInputItems(IList<InputItem> inputItems)
        {
            var indexedInputItems = new List<IIndexed<InputItem>>();

            var index = 0;
            foreach (var inputItem in inputItems)
            {
                indexedInputItems.Add(new Indexed<InputItem>(inputItem, index));
                index++;
            }

            return indexedInputItems;
        }

        /// <summary>Log results of input conversion.</summary>
        /// <param name="lengthOfInput">The total length of input. </param>
        /// <param name="successes">The successes. </param>
        /// <param name="failures">The failures. </param>
        /// <typeparam name="T">The type of the value </typeparam>
        private static void LogResultsOfInputConversion<TIn, TOut>(
            int lengthOfInput, ICollection<IIndexed<TIn>> successes, ICollection<IIndexed<TOut>> failures)
        {
            var goodCount = successes.Count;
            var badCount = failures.Count;
            var isOnlyOne = lengthOfInput == 1;

            if (lengthOfInput == goodCount && badCount == 0)
            {
                Debug.WriteLine("Successfully converted {0} item{1}.", lengthOfInput, lengthOfInput > 1);
            }

            if (badCount == lengthOfInput && goodCount == 0)
            {
                Debug.WriteLine(
                    "Unsuccessfully attempted to convert all {0} item{1}.", 
                    lengthOfInput, 
                    isOnlyOne ? string.Empty : "s");

                // We could add the first reason here.
            }

            if (badCount + goodCount == lengthOfInput)
            {
                Debug.WriteLine("Unsuccessfully converted {0} item{1}.", badCount, badCount > 1);

                // Since there are some that worked and others that did not, lets log all the reasons (Even better would be the input that failed, but...)
            }

            if (badCount + goodCount != lengthOfInput)
            {
                Debug.WriteLine(
                    "There is a mismatch between the number of inputs:{0}, and the number of results, success: {1}, failed: {2}.", 
                    lengthOfInput, 
                    goodCount, 
                    badCount);
            }
        }

        /// <summary>The process input conversions.</summary>
        /// <param name="inputConverter">The input converter.</param>
        /// <param name="indexedInputItems">The indexed input items.</param>
        /// <param name="indexedErrorResultItems">The indexed error result items.</param>
        /// <typeparam name="TIn"></typeparam>
        /// <returns>The <see cref="IList"/>.</returns>
        private static IList<IIndexed<TIn>> ProcessInputConversions<TIn>(
            Func<InputItem, TIn> inputConverter, 
            IList<IIndexed<InputItem>> indexedInputItems, 
            out IList<IIndexed<ResultItem>> indexedErrorResultItems)
        {
            var convertedIndexedInputItems = new List<IIndexed<TIn>>();
            indexedErrorResultItems = new List<IIndexed<ResultItem>>();

            var sw = new Stopwatch();
            sw.Start();

            foreach (var indexedInputItem in indexedInputItems)
            {
                try
                {
                    var item = inputConverter(indexedInputItem.Value);
                    convertedIndexedInputItems.Add(new Indexed<TIn>(item, indexedInputItem.Index));
                }
                catch (Exception e)
                {
                    var resultItem = new ResultItem(0, true, e);
                    var indexedError = new Indexed<ResultItem>(resultItem, indexedInputItem.Index);

                    indexedErrorResultItems.Add(indexedError);
                }
            }

            sw.Stop();
            Debug.WriteLine(
                string.Format("Input conversion: Converting the input took {0} milliseconds.", sw.ElapsedMilliseconds));
            Logger.Write(
                Logger.Severity.Debug, 
                "Input conversion", 
                string.Format("Converting the input took {0} milliseconds.", sw.ElapsedMilliseconds));

            return convertedIndexedInputItems;
        }

        /// <summary>The process results.</summary>
        /// <param name="task">The task.</param>
        /// <param name="resultConverter">The result converter.</param>
        /// <param name="chunk">The chunk.</param>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <returns>The <see cref="List"/>.</returns>
        private static List<IIndexed<ResultItem>> ProcessResults<TIn, TOut>(
            Task<IList<TOut>> task, Func<TOut, ResultItem> resultConverter, List<IIndexed<TIn>> chunk)
        {
            var executionResults = new List<IIndexed<ResultItem>>();

            if (task.Exception != null)
            {
                var exception = new FatalErrorException(Resources.Connector.OperationFailed, task.Exception.Flatten());

                Logger.Write(Logger.Severity.Error, Resources.Connector.OperationFailed, exception.InnerException.Message);

                var errorResultItem = new ResultItem(0, true, exception);

                var indexedErrors = chunk.Select(i => new Indexed<ResultItem>(errorResultItem, i.Index));

                executionResults.AddRange(indexedErrors);
            }
            else
            {
                executionResults.AddRange(ConvertResultsInternal(resultConverter, chunk, task.Result));
            }

            return executionResults;
        }

        /// <summary>The validate final results.</summary>
        /// <param name="finalResults">The final results.</param>
        /// <exception cref="FatalErrorException"></exception>
        private static void ValidateFinalResults(IList<ResultItem> finalResults)
        {
            if (finalResults.All(finalResult => finalResult.HasError && finalResult.IsFatalError) && finalResults.Count > 0)
            {
                var msg = string.Format("All {0} records failed processing.", finalResults.Count);
                throw new FatalErrorException(msg, finalResults.First().Error.InnerException);
            }
        }

        /// <summary>The wait for tasks to complete.</summary>
        /// <param name="tasks">The tasks.</param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="FatalErrorException"></exception>
        private static void WaitForTasksToComplete<T>(Task<T>[] tasks)
        {
            var sw = new Stopwatch();
            sw.Start();

            if (tasks.Length > 0)
            {
                try
                {
                    Task.WaitAll(tasks);
                }
                catch (AggregateException ae)
                {
                    throw new FatalErrorException("A fatal error occured while processing operation.", ae.Flatten());
                }
            }

            sw.Stop();


            var msg = string.Format(Resources.Connector.LogExecuteOperationComplete, sw.ElapsedMilliseconds);
            Debug.WriteLine(msg);
            Logger.Write(Logger.Severity.Debug, "Executing operation(s)", msg);
        }

        internal static IList<ResultItem> Execute<TIn, TOut>(IList<InputItem> input, Func<InputItem, TIn> func1, Func<TIn, TOut> func2, Func<TOut, ResultItem> func3)
        {
            throw new NotImplementedException();
        }
    }
}