namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class DateTimeProperty : SimpleProperty<DateTime>
    {
        public DateTimeProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        protected override DateTime Convert(object obj)
        {
            if (obj is DateTime x) return x;
            return SimpleTypeConverters.ConvertToDateTime(obj);
        }
    }
}