namespace Scribe.Connector.Common.Reflection.Actions
{
    using Scribe.Core.ConnectorApi.Metadata;

    public class UpsertAction : ActionDef
    {
        public UpsertAction(string name, string description, bool supportsBulk)
            : base(name, description, KnownActions.UpdateWith, supportsBulk, false)
        {
        }
    }
}