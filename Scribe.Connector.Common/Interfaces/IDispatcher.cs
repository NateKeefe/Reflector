// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDispatcher.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The Dispatcher interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Interfaces
{
    using Scribe.Core.ConnectorApi;
    using Scribe.Core.ConnectorApi.Actions;
    using Scribe.Core.ConnectorApi.Query;

    /// <summary>The Dispatcher interface.</summary>
    /// <typeparam name="T"></typeparam>
    public interface IDispatcher<T>
    {
        /// <summary>Gets the connection.</summary>
        IConnection<T> Connection { get; }

        /// <summary>The get metadata provider.</summary>
        /// <returns>The <see cref="IMetadataProvider"/>.</returns>
        IMetadataProvider GetMetadataProvider();

        /// <summary>The get method handler.</summary>
        /// <param name="input">The input.</param>
        /// <returns>The <see cref="IMethodHandler"/>.</returns>
        IMethodHandler GetMethodHandler(MethodInput input);

        /// <summary>The get operation handler.</summary>
        /// <param name="operationName">The operation name.</param>
        /// <param name="entityName">The entity name.</param>
        /// <param name="allowMultiple">The allow multiple.</param>
        /// <returns>The <see cref="IOperationHandler"/>.</returns>
        IOperationHandler GetOperationHandler(string operationName, string entityName, bool allowMultiple);

        /// <summary>The get query handler.</summary>
        /// <param name="query">The query.</param>
        /// <returns>The <see cref="IQueryHandler"/>.</returns>
        IQueryHandler GetQueryHandler(Query query);
    }
}