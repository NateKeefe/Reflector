namespace Scribe.Connector.Common.Reflection
{
    using System;

    using Scribe.Connector.Common.Reflection.Actions;
    using Scribe.Core.ConnectorApi.Metadata;

    public abstract class SupportedOperationAttribute : Attribute
    {
        public abstract ActionDef ToActionDefinition();
    }
}