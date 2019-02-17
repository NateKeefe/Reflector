namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NInt32ListProperty : SimpleNullableListProperty<int>
    {
        public NInt32ListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Int32? ConvertItem(object obj)
        {
            return SimpleTypeConverters.ConvertToInt32(obj);
        }
    }
}