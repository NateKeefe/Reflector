namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NDateTimeListProperty : SimpleNullableListProperty<DateTime>
    {
        public NDateTimeListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override DateTime? ConvertItem(object obj)
        {
            return SimpleTypeConverters.ConvertToDateTime(obj);
        }
    }
}