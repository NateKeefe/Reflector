// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryHandler.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The query handler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Scribe.Connector.Common.Interfaces;
using Scribe.Core.ConnectorApi;

namespace Scribe.Connector.Common.Queries
{
    /// <summary>The query handler.</summary>
    /// <typeparam name="TNativeQuery"></typeparam>
    /// <typeparam name="TNativeResult"></typeparam>
    public class QueryHandler<TNativeQuery, TNativeResult> : IQueryHandler
    {
        /// <summary>The convert query.</summary>
        private readonly Func<Core.ConnectorApi.Query.Query, TNativeQuery> ConvertQuery;

        /// <summary>The convert query result item.</summary>
        private readonly Func<TNativeResult, DataEntity> ConvertQueryResultItem;

        /// <summary>The execute query.</summary>
        private readonly Func<TNativeQuery, IEnumerable<TNativeResult>> ExecuteQuery;

        /// <summary>Initializes a new instance of the <see cref="QueryHandler{TNativeQuery,TNativeResult}"/> class.</summary>
        /// <param name="convertQuery">The convert query.</param>
        /// <param name="executeQuery">The execute query.</param>
        /// <param name="convertQueryResultItem">The convert query result item.</param>
        protected QueryHandler(
            Func<Core.ConnectorApi.Query.Query, TNativeQuery> convertQuery, 
            Func<TNativeQuery, IEnumerable<TNativeResult>> executeQuery, 
            Func<TNativeResult, DataEntity> convertQueryResultItem)
        {
            this.ConvertQuery = convertQuery;
            this.ExecuteQuery = executeQuery;
            this.ConvertQueryResultItem = convertQueryResultItem;
        }

        /// <summary>The build query handler.</summary>
        /// <param name="convertQuery">The convert query.</param>
        /// <param name="executeQuery">The execute query.</param>
        /// <param name="convertQueryResultItem">The convert query result item.</param>
        /// <typeparam name="TNativeQuery"></typeparam>
        /// <typeparam name="TNativeResult"></typeparam>
        /// <returns>The <see cref="QueryHandler"/>.</returns>
        public static QueryHandler<TNativeQuery, TNativeResult> BuildQueryHandler(
            Func<Core.ConnectorApi.Query.Query, TNativeQuery> convertQuery, 
            Func<TNativeQuery, IEnumerable<TNativeResult>> executeQuery, 
            Func<TNativeResult, DataEntity> convertQueryResultItem)
        {
            return new QueryHandler<TNativeQuery, TNativeResult>(convertQuery, executeQuery, convertQueryResultItem);
        }

        /// <summary>The execute.</summary>
        /// <param name="query">The query.</param>
        /// <returns>The <see cref="IEnumerable"/>.</returns>
        public IEnumerable<DataEntity> Execute(Core.ConnectorApi.Query.Query query)
        {
            var nativeQuery = this.ConvertQuery(query);

            var enumerable = this.ExecuteQuery(nativeQuery);

            return enumerable.Select(nativeResult => this.ConvertQueryResultItem(nativeResult));
        }
    }
}