namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NDecimalListProperty : SimpleNullableListProperty<decimal>
    {
        public NDecimalListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Decimal? ConvertItem(object obj)
        {
            return SimpleTypeConverters.ConvertToDecimal(obj);
        }
    }
}