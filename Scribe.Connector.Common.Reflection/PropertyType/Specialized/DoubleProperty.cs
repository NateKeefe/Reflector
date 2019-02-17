namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class DoubleProperty : SimpleProperty<double>
    {
        public DoubleProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        protected override Double Convert(object obj)
        {
            if (obj is Double x) return x;
            return SimpleTypeConverters.ConvertToDouble(obj);
        }
    }
}