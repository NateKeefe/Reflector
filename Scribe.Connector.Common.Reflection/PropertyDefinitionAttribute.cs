namespace Scribe.Connector.Common.Reflection
{
    using System;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PropertyDefinitionAttribute : Attribute, ISettablePropertyDef
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsPrimaryKey { get; set; }

        public bool UsedInQuerySelect { get; set; } = true;

        public bool UsedInQueryConstraint { get; set; }

        public bool UsedInActionInput { get; set; } = true;

        public bool UsedInActionOutput { get; set; } = true;

        public bool UsedInLookupCondition { get; set; }

        public bool UsedInQuerySequence { get; set; } = true;

        public bool RequiredInActionInput { get; set; }
    }
}