namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using System;

    using Scribe.Connector.Common.Reflection;

    public abstract class SimpleNullableProperty<T> : SimpleProperty<T?> where T : struct
    {
        public SimpleNullableProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        public override string PropertyType { get; } = typeof(T).FullName;

        public override bool IsCollection { get; } = false;
    }
}