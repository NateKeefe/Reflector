namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NBooleanProperty : SimpleNullableProperty<bool>
    {
        public NBooleanProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        protected override Boolean? Convert(object obj)
        {
            if (obj == null) return null;
            if (obj is Boolean x) return x;
            return SimpleTypeConverters.ConvertToBoolean(obj);
        }
    }
}