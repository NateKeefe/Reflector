namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class Int32ListProperty : SimpleListProperty<int>
    {
        public Int32ListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Int32 ConvertItem(object obj)
        {
            if (obj is Int32 x) return x;
            return SimpleTypeConverters.ConvertToInt32(obj);
        }
    }
}