namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class Int64ListProperty : SimpleListProperty<long>
    {
        public Int64ListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Int64 ConvertItem(object obj)
        {
            if (obj is Int64 x) return x;
            return SimpleTypeConverters.ConvertToInt64(obj);
        }
    }
}