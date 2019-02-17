namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class StringProperty : SimpleProperty<string>
    {
        public StringProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        protected override String Convert(object obj)
        {
            if (obj is String x) return x;
            return SimpleTypeConverters.ConvertToString(obj);
        }
    }
}