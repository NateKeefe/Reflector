namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NCharListProperty : SimpleNullableListProperty<char>
    {
        public NCharListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Char? ConvertItem(object obj)
        {
            return SimpleTypeConverters.ConvertToChar(obj);
        }
    }
}