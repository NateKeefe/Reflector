namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NByteListProperty : SimpleNullableListProperty<byte>
    {
        public NByteListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Byte? ConvertItem(object obj)
        {
            return SimpleTypeConverters.ConvertToByte(obj);
        }
    }
}