namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class ByteProperty : SimpleProperty<byte>
    {
        public ByteProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        protected override Byte Convert(object obj)
        {
            if (obj is Byte x) return x;
            return SimpleTypeConverters.ConvertToByte(obj);
        }
    }

    internal sealed class ByteArrayProperty : SimpleProperty<Byte[]>
    {
        public ByteArrayProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        protected override Byte[] Convert(object obj)
        {
            if (obj is Byte[] x) return x;
            return SimpleTypeConverters.ConvertToByteArray(obj);
        }
    }
}