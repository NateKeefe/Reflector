namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class DecimalProperty : SimpleProperty<decimal>
    {
        public DecimalProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        protected override Decimal Convert(object obj)
        {
            if (obj is Decimal x) return x;
            return SimpleTypeConverters.ConvertToDecimal(obj);
        }
    }
}