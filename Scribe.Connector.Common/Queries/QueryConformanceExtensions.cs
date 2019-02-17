namespace Scribe.Connector.Common.Queries
{
    using Scribe.Core.ConnectorApi;
    using Scribe.Core.ConnectorApi.Query;

    public static class QueryConformanceExtensions
    {
        public static DataEntity Conform(this DataEntity entity, Query query)
        {
            return QueryConformance.Conform(entity, query);
        }
    }
}