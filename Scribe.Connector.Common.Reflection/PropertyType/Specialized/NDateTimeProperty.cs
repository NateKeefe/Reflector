namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NDateTimeProperty : SimpleNullableProperty<DateTime>
    {
        public NDateTimeProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        protected override DateTime? Convert(object obj)
        {
            if (obj == null) return null;
            if (obj is DateTime x) return x;
            return SimpleTypeConverters.ConvertToDateTime(obj);
        }
    }
}