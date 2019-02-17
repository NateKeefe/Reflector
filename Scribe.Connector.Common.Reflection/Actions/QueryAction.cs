namespace Scribe.Connector.Common.Reflection.Actions
{
    using Scribe.Core.ConnectorApi.Metadata;

    public class QueryAction : ActionDef
    {
        public QueryAction()
            : base("Query", "Query", KnownActions.Query, false, false)
        {
        }
    }
}