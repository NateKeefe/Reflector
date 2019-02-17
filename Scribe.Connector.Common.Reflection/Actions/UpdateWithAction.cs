namespace Scribe.Connector.Common.Reflection.Actions
{
    using Scribe.Core.ConnectorApi.Metadata;

    public class UpdateWithAction : ActionDef
    {
        public UpdateWithAction(string name, string description, bool supportsBulk, bool supportsMulti)
            : base(name, description, KnownActions.UpdateWith, supportsBulk, supportsMulti)
        {
        }
    }
}