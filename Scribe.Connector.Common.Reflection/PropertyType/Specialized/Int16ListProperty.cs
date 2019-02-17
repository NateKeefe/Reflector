namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class Int16ListProperty : SimpleListProperty<short>
    {
        public Int16ListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Int16 ConvertItem(object obj)
        {
            if (obj is Int16 x) return x;
            return SimpleTypeConverters.ConvertToInt16(obj);
        }
    }
}