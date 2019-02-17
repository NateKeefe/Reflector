namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class DateTimeListProperty : SimpleListProperty<DateTime>
    {
        public DateTimeListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override DateTime ConvertItem(object obj)
        {
            if (obj is DateTime x) return x;
            return SimpleTypeConverters.ConvertToDateTime(obj);
        }
    }
}