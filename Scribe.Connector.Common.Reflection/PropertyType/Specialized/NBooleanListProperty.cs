namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    using static SimpleTypeConverters;
    internal sealed class NBooleanListProperty : SimpleNullableListProperty<Boolean>
    {
        public NBooleanListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Boolean? ConvertItem(object obj)
        {
            return SimpleTypeConverters.ConvertToBoolean(obj);
        }
    }
}
