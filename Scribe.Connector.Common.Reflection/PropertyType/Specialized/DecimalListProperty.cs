namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class DecimalListProperty : SimpleListProperty<decimal>
    {
        public DecimalListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Decimal ConvertItem(object obj)
        {
            if (obj is Decimal x) return x;
            return SimpleTypeConverters.ConvertToDecimal(obj);
        }
    }
}