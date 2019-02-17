namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NDecimalProperty : SimpleNullableProperty<decimal>
    {
        public NDecimalProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        protected override Decimal? Convert(object obj)
        {
            if (obj == null) return null;
            if (obj is Decimal x) return x;
            return SimpleTypeConverters.ConvertToDecimal(obj);
        }
    }
}