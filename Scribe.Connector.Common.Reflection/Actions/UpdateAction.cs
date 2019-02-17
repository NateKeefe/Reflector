namespace Scribe.Connector.Common.Reflection.Actions
{
    using Scribe.Core.ConnectorApi.Metadata;

    public class UpdateAction : ActionDef
    {
        public UpdateAction(string name, string description, bool supportsBulk, bool supportsMulti)
            : base(name, description, KnownActions.Update, supportsBulk, supportsMulti)
        {
        }
    }
}