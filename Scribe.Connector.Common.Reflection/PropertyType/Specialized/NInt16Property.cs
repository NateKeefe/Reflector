namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NInt16Property : SimpleNullableProperty<short>
    {
        public NInt16Property(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        protected override Int16? Convert(object obj)
        {
            if (obj == null) return null;
            if (obj is Int16 x) return x;
            return SimpleTypeConverters.ConvertToInt16(obj);
        }
    }
}