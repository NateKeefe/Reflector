namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class BooleanListProperty : SimpleListProperty<bool>
    {
        public BooleanListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Boolean ConvertItem(object obj)
        {
            if (obj is Boolean x) return x;
            return SimpleTypeConverters.ConvertToBoolean(obj);
        }
    }
}