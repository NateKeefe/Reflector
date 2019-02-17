namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class DoubleListProperty : SimpleListProperty<double>
    {
        public DoubleListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Double ConvertItem(object obj)
        {
            if (obj is Double x) return x;
            return SimpleTypeConverters.ConvertToDouble(obj);
        }
    }
}