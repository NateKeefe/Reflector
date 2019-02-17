namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NInt32Property : SimpleNullableProperty<int>
    {
        public NInt32Property(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        protected override Int32? Convert(object obj)
        {
            if (obj == null) return null;
            if (obj is Int32 x) return x;
            return SimpleTypeConverters.ConvertToInt32(obj);
        }
    }
}