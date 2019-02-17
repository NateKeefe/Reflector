// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOperationHandler.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The Interface used to handle operations
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Scribe.Connector.Common.Operation;

namespace Scribe.Connector.Common.Interfaces
{
    using System.Collections.Generic;

    using Scribe.Connector.Common;

    /// <summary>
    ///   The Interface used to handle operations
    /// </summary>
    public interface IOperationHandler
    {
        /// <summary>The executecution of the operation</summary>
        /// <param name="input">The indexed list of data entities </param>
        /// <returns>List of operation results. </returns>
        IList<ResultItem> Execute(IList<InputItem> input);
    }
}