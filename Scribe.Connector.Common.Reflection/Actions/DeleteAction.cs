namespace Scribe.Connector.Common.Reflection.Actions
{
    using Scribe.Core.ConnectorApi.Metadata;

    public class DeleteAction : ActionDef
    {
        public DeleteAction(string name, string description, bool supportsBulk, bool supportsMulti)
            : base(name, description, KnownActions.Delete, supportsBulk, supportsMulti)
        {
        }
    }
}