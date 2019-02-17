namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NInt64Property : SimpleNullableProperty<long>
    {
        public NInt64Property(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        protected override Int64? Convert(object obj)
        {
            if (obj == null) return null;
            if (obj is Int64 x) return x;
            return SimpleTypeConverters.ConvertToInt64(obj);
        }
    }
}