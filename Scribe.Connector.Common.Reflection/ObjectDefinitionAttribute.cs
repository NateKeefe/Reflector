namespace Scribe.Connector.Common.Reflection
{
    using System;

    using Scribe.Connector.Common.Reflection.PropertyType;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = false)]
    public class ObjectDefinitionAttribute : Attribute
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool Hidden { get; set; } = false;
    }
}