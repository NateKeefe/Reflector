namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NInt16ListProperty : SimpleNullableListProperty<short>
    {
        public NInt16ListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Int16? ConvertItem(object obj)
        {
            return SimpleTypeConverters.ConvertToInt16(obj);
        }
    }
}