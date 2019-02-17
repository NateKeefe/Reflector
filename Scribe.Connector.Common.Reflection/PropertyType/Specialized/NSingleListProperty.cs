namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NSingleListProperty : SimpleNullableListProperty<float>
    {
        public NSingleListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Single? ConvertItem(object obj)
        {
            return SimpleTypeConverters.ConvertToSingle(obj);
        }
    }
}