namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using System;
    using System.Reflection;

    public class DataEntityPropertyDef : PropDefBase // This is the first pass
    {
        public DataEntityPropertyDef(ISettablePropertyDef pDef, string fieldName,  Type dataEntityType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, fieldName)
        {
            var attr = dataEntityType.GetCustomAttribute<ObjectDefinitionAttribute>();
            this.PropertyType = string.IsNullOrWhiteSpace(attr.Name) ? dataEntityType.Name : attr.Name;
            this.DataEntityType = dataEntityType;
            this.getter = getter;
            this.setter = setter;
        }

        public Func<object, object> getter { get; }

        public Action<object, object> setter { get; }

        public override bool IsObjectDefProp { get; } = true;

        public override string PropertyType { get; }

        public Type DataEntityType { get; }
    }
    public class DataEntityListPropertyDef : PropDefBase // This is the first pass
    {
        public DataEntityListPropertyDef(ISettablePropertyDef pDef, string fieldName, Type dataEntityType, Func<object, object> getter, Action<object, object> setter, Type dotNetPropertyType)
            : base(pDef, fieldName)
        {
            var attr = dataEntityType.GetCustomAttribute<ObjectDefinitionAttribute>();
            this.PropertyType = string.IsNullOrWhiteSpace(attr.Name) ? dataEntityType.Name : attr.Name;
            this.DataEntityType = dataEntityType;
            this.getter = getter;
            this.setter = setter;
            this.DotNetPropertyType = dotNetPropertyType;
        }

        public Func<object, object> getter { get; }

        public Action<object, object> setter { get; }

        public override bool IsObjectDefProp { get; } = true;

        public override bool IsCollection { get; } = true;

        public override string PropertyType { get; }

        public Type DataEntityType { get; }
        public Type DotNetPropertyType { get; }
    }

}