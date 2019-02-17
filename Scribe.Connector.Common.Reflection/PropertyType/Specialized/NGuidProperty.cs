namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NGuidProperty : SimpleNullableProperty<Guid>
    {
        public NGuidProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter) : base(pDef, propName, propType, getter, setter) { }

        protected override Guid? Convert(object obj)
        {
            if (obj == null) return null;
            if (obj is Guid x) return x;
            return SimpleTypeConverters.ConvertToGuid(obj);
        }
    }
}