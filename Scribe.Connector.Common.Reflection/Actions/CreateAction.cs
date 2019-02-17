namespace Scribe.Connector.Common.Reflection.Actions
{
    using Scribe.Core.ConnectorApi.Metadata;

    public class CreateAction : ActionDef
    {
        public CreateAction(string name, string description, bool supportsBulk)
            : base(name, description, KnownActions.Create, supportsBulk, false)
        {
        }
    }
}