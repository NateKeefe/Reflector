namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class CharProperty : SimpleProperty<char>
    {
        public CharProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        protected override Char Convert(object obj)
        {
            if (obj is Char x) return x;
            return SimpleTypeConverters.ConvertToChar(obj);
        }
    }
}