namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NDoubleListProperty : SimpleNullableListProperty<double>
    {
        public NDoubleListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Double? ConvertItem(object obj)
        {
            return SimpleTypeConverters.ConvertToDouble(obj);
        }
    }
}