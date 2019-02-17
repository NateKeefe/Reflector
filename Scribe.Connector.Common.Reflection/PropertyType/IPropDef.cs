namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using Scribe.Connector.Common.Reflection;
    using Scribe.Core.ConnectorApi.Metadata;

    public interface IPropDef : ISettablePropertyDef
    {
        bool IsCollection { get; }
        bool IsObjectDefProp { get; }
        IPropertyDefinition ToPropertyDefinition();
    }
}