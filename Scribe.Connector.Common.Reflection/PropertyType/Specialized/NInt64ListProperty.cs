namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NInt64ListProperty : SimpleNullableListProperty<long>
    {
        public NInt64ListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Int64? ConvertItem(object obj)
        {
            return SimpleTypeConverters.ConvertToInt64(obj);
        }
    }
}