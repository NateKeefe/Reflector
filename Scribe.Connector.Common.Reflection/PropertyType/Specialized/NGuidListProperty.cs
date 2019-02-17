namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class NGuidListProperty : SimpleNullableListProperty<Guid>
    {
        public NGuidListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Guid? ConvertItem(object obj)
        {
            return SimpleTypeConverters.ConvertToGuid(obj);
        }
    }
}