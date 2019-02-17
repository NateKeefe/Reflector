namespace Scribe.Connector.Common.Reflection
{
    using Scribe.Core.ConnectorApi.Metadata;



    public abstract class PropertyDef : ISettablePropertyDef
    {
        private readonly bool isCollection;
        public string PropertyType { get; }

        public PropertyDef(
            string name,
            string description,
            string propertyType,
            bool isPrimaryKey,
            bool usedInQuerySelect,
            bool usedInQueryConstraint,
            bool usedInActionInput,
            bool usedInActionOutput,
            bool usedInLookupCondition,
            bool usedInQuerySequence,
            bool requiredInActionInput,
            bool isCollection)
        {
            this.Name = name;
            this.Description = description;
            this.PropertyType = propertyType;
            this.IsPrimaryKey = isPrimaryKey;
            this.UsedInQuerySelect = usedInQuerySelect;
            this.UsedInQueryConstraint = usedInQueryConstraint;
            this.UsedInActionInput = usedInActionInput;
            this.UsedInActionOutput = usedInActionOutput;
            this.UsedInLookupCondition = usedInLookupCondition;
            this.UsedInQuerySequence = usedInQuerySequence;
            this.RequiredInActionInput = requiredInActionInput;
            this.isCollection = isCollection;
        }

        public string Name { get; }

        public string Description { get; }

        public bool IsPrimaryKey { get; }

        public bool UsedInQuerySelect { get; }

        public bool UsedInQueryConstraint { get; }

        public bool UsedInActionInput { get; }

        public bool UsedInActionOutput { get; }

        public bool UsedInLookupCondition { get; }

        public bool UsedInQuerySequence { get; }

        public bool RequiredInActionInput { get; }

        public IPropertyDefinition AsPropertyDefinition()
        {
            return new PropertyDefinition
                       {
                           // Settable
                           FullName = this.Name,
                           Name = this.Name,
                           Description = string.IsNullOrWhiteSpace(this.Description) ? string.Empty : this.Description,
                           IsPrimaryKey = this.IsPrimaryKey,
                           MaxOccurs = this.isCollection ? -1 : 1,
                           PropertyType = this.PropertyType,
                           RequiredInActionInput = this.RequiredInActionInput,
                           UsedInActionInput = this.UsedInActionInput,
                           UsedInActionOutput = this.UsedInActionInput || this.UsedInActionOutput,
                           UsedInLookupCondition = this.UsedInLookupCondition,
                           UsedInQueryConstraint = this.UsedInQueryConstraint,
                           UsedInQuerySelect = this.UsedInQuerySelect,
                           UsedInQuerySequence = this.UsedInQuerySequence,

                           // Not settable
                           MinOccurs = this.RequiredInActionInput ? 1 : 0,
                           Nullable = !this.RequiredInActionInput,
                           NumericPrecision = 0,
                           NumericScale = 0,
                           PresentationType = string.Empty,
                           Size = 0 // Theoretically, we could make this settable as well
                       };
        }
    }
}