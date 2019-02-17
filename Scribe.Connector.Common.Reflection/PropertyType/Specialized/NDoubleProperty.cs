namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NDoubleProperty : SimpleNullableProperty<double>
    {
        public NDoubleProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        protected override Double? Convert(object obj)
        {
            if (obj == null) return null;
            if (obj is Double x) return x;
            return SimpleTypeConverters.ConvertToDouble(obj);
        }
    }
}