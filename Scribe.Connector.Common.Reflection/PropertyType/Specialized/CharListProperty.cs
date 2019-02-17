namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class CharListProperty : SimpleListProperty<char>
    {
        public CharListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Char ConvertItem(object obj)
        {
            if (obj is Char x) return x;
            return SimpleTypeConverters.ConvertToChar(obj);
        }
    }
}