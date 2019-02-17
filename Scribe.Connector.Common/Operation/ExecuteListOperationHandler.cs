using System;
using System.Collections.Generic;
using Scribe.Connector.Common.Interfaces;

namespace Scribe.Connector.Common.Operation
{
    /// <summary>The operation handler.</summary>
    /// <typeparam name="TNative"></typeparam>
    /// <typeparam name="TNativeOut"></typeparam>
    public class ExecuteListOperationHandler<TNative, TNativeOut> : IOperationHandler
    {
        /// <summary>The _execute.</summary>
        private readonly Func<IList<TNative>, IList<TNativeOut>> _execute;

        /// <summary>The _input converter.</summary>
        private readonly Func<InputItem, TNative> _inputConverter;

        /// <summary>The _output converter.</summary>
        private readonly Func<TNativeOut, ResultItem> _outputConverter;

        private readonly int _batchSize;

        /// <summary>Initializes a new instance of the <see cref="ExecuteListOperationHandler{TNative,TNativeOut}"/> class. 
        /// Initializes a new instance of the <see cref="ExecuteListOperationHandler{TInput,TResult}"/> class.</summary>
        /// <param name="inputConverter">TODO The input converter. </param>
        /// <param name="execute">TODO The execute. </param>
        /// <param name="outputConverter">TODO The output converter. </param>
        public ExecuteListOperationHandler(
            Func<InputItem, TNative> inputConverter, 
            Func<IList<TNative>, IList<TNativeOut>> execute, 
            Func<TNativeOut, ResultItem> outputConverter)
        {
            this._inputConverter = inputConverter;
            this._execute = execute;
            this._outputConverter = outputConverter;
        }

        /// <summary>Initializes a new instance of the <see cref="ExecuteListOperationHandler{TNative,TNativeOut}"/> class. 
        /// Initializes a new instance of the <see cref="ExecuteListOperationHandler{TInput,TResult}"/> class.</summary>
        /// <param name="inputConverter">TODO The input converter. </param>
        /// <param name="execute">TODO The execute. </param>
        /// <param name="outputConverter">TODO The output converter. </param>
        /// <param name="batchSize"></param>
        public ExecuteListOperationHandler(
            Func<InputItem, TNative> inputConverter,
            Func<IList<TNative>, IList<TNativeOut>> execute,
            Func<TNativeOut, ResultItem> outputConverter,
            int batchSize)
            : this(inputConverter, execute, outputConverter)
        {
            this._batchSize = batchSize;
        }

        /// <summary>The executecution of the operation</summary>
        /// <param name="input">The indexed list of data entities </param>
        /// <returns>List of operation results. </returns>
        public IList<ResultItem> Execute(IList<InputItem> input)
        {
            IList<ResultItem> results;

            if (_batchSize > 0)
            {
                results = ExecutionFlow.Execute(input, this._inputConverter, this._execute, this._outputConverter, _batchSize);
            }
            else
            {
                results = ExecutionFlow.Execute(input, this._inputConverter, this._execute, this._outputConverter);
            }

            return results;
        }
    }
}