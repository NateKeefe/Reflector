namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using Scribe.Core.ConnectorApi.Metadata;

    public abstract class PropDefBase : IPropDef
    {
        public PropDefBase(ISettablePropertyDef pDef, string fieldName)
        {
            this.Name = string.IsNullOrWhiteSpace(pDef.Name) ? fieldName : pDef.Name.Trim();
            this.Description = string.IsNullOrWhiteSpace(pDef.Description) ? string.Empty : pDef.Description.Trim();
            this.IsPrimaryKey = pDef.IsPrimaryKey;
            this.RequiredInActionInput = pDef.RequiredInActionInput;
            this.UsedInActionInput = pDef.UsedInActionInput;
            this.UsedInActionInput = pDef.UsedInActionInput;
            this.UsedInActionOutput = pDef.UsedInActionOutput;
            this.UsedInLookupCondition = pDef.UsedInLookupCondition;
            this.UsedInQueryConstraint = pDef.UsedInQueryConstraint;
            this.UsedInQuerySelect = pDef.UsedInQuerySelect;
            this.UsedInQuerySequence = pDef.UsedInQuerySequence;
        }


        public PropDefBase(ISettablePropertyDef pDef, string fieldName, string description)
        {
            this.Name = string.IsNullOrWhiteSpace(pDef.Name) ? fieldName : pDef.Name.Trim();
            this.Description = description;
            this.IsPrimaryKey = pDef.IsPrimaryKey;
            this.RequiredInActionInput = pDef.RequiredInActionInput;
            this.UsedInActionInput = pDef.UsedInActionInput;
            this.UsedInActionInput = pDef.UsedInActionInput;
            this.UsedInActionOutput = pDef.UsedInActionOutput;
            this.UsedInLookupCondition = pDef.UsedInLookupCondition;
            this.UsedInQueryConstraint = pDef.UsedInQueryConstraint;
            this.UsedInQuerySelect = pDef.UsedInQuerySelect;
            this.UsedInQuerySequence = pDef.UsedInQuerySequence;
        }

        public string Description { get; }

        public virtual bool IsCollection { get; } = false;

        public abstract bool IsObjectDefProp { get; }

        public bool IsPrimaryKey { get; }

        public string Name { get; }

        public abstract string PropertyType { get; }

        public bool RequiredInActionInput { get; }

        public bool UsedInActionInput { get; }

        public bool UsedInActionOutput { get; }

        public bool UsedInLookupCondition { get; }

        public bool UsedInQueryConstraint { get; }

        public bool UsedInQuerySelect { get; }

        public bool UsedInQuerySequence { get; }

        public IPropertyDefinition ToPropertyDefinition()
        {
            return new PropertyDefinition
                       {
                           // Settable
                           FullName = this.Name,
                           Name = this.Name,
                           Description =
                               string.IsNullOrWhiteSpace(this.Description)
                                   ? string.Empty
                                   : this.Description,
                           IsPrimaryKey = this.IsPrimaryKey,
                           MaxOccurs = this.IsCollection ? -1 : 1,
                           PropertyType = this.PropertyType,
                           RequiredInActionInput = this.RequiredInActionInput,
                           UsedInActionInput = this.UsedInActionInput,
                           UsedInActionOutput =
                               this.UsedInActionInput || this.UsedInActionOutput,
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