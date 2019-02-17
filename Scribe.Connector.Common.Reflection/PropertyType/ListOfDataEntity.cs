namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using System;

    public class ListOfDataEntity : PropDefBase
    {
        public ListOfDataEntity(ISettablePropertyDef pDef, IObjDefHeader header, string propName, Type underlyingType)
            : base(pDef, propName)
        {
            this.PropertyType = string.IsNullOrWhiteSpace(header.Name) ? underlyingType.Name : header.Name.Trim();
        }

        public override bool IsCollection { get; } = true;

        public override bool IsObjectDefProp { get; } = true;

        public override string PropertyType { get; }
    }
}