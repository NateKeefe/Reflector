namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class ByteListProperty : SimpleListProperty<byte>
    {
        public ByteListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Byte ConvertItem(object obj)
        {
            if (obj is Byte x) return x;
            return SimpleTypeConverters.ConvertToByte(obj);
        }
    }
}