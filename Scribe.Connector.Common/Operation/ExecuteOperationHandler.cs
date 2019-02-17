// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationHandler.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The operation handler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Scribe.Connector.Common.Interfaces;

namespace Scribe.Connector.Common.Operation
{
    public class ExecuteOperationHandler<TNative, TNativeOut> : IOperationHandler
    {
        /// <summary>The _execute.</summary>
        private readonly Func<TNative, TNativeOut> _execute;

        /// <summary>The _input converter.</summary>
        private readonly Func<InputItem, TNative> _inputConverter;

        /// <summary>The _output converter.</summary>
        private readonly Func<TNativeOut, ResultItem> _outputConverter;

        public ExecuteOperationHandler(
            Func<InputItem, TNative> inputConverter, 
            Func<TNative, TNativeOut> execute, 
            Func<TNativeOut, ResultItem> outputConverter)
        {
            this._inputConverter = inputConverter;
            this._execute = execute;
            this._outputConverter = outputConverter;
        }

        /// <summary>The executecution of the operation</summary>
        /// <param name="input">The indexed list of data entities </param>
        /// <returns>List of operation results. </returns>
        public IList<ResultItem> Execute(IList<InputItem> input)
        {
            return ExecutionFlow.Execute(input, this._inputConverter, this._execute, this._outputConverter);
        }
    }
}