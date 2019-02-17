namespace Scribe.Connector.Common.Reflection.Actions
{
    using Scribe.Core.ConnectorApi.Metadata;

    public class NativeQueryAction : ActionDef
    {
        public NativeQueryAction()
            : base("NativeQuery", "NativeQuery", KnownActions.NativeQuery, false, false)
        {
        }
    }
}