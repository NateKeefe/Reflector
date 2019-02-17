namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NSingleProperty : SimpleNullableProperty<float>
    {
        public NSingleProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        protected override Single? Convert(object obj)
        {
            if (obj == null) return null;
            if (obj is Single x) return x;
            return SimpleTypeConverters.ConvertToSingle(obj);
        }
    }
}