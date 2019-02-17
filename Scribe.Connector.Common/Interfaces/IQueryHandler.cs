// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IQueryHandler.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The QueryHandler interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Interfaces
{
    using System.Collections.Generic;

    using Scribe.Core.ConnectorApi;
    using Scribe.Core.ConnectorApi.Query;

    /// <summary>The QueryHandler interface.</summary>
    public interface IQueryHandler
    {
        /// <summary>The execute.</summary>
        /// <param name="query">The query.</param>
        /// <returns>The <see cref="IEnumerable"/>.</returns>
        IEnumerable<DataEntity> Execute(Query query);
    }
}