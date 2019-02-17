namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NByteProperty : SimpleNullableProperty<byte>
    {
        public NByteProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        protected override Byte? Convert(object obj)
        {
            if (obj == null) return null;
            if (obj is Byte x) return x;
            return SimpleTypeConverters.ConvertToByte(obj);
        }
    }
}