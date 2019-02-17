namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class GuidListProperty : SimpleListProperty<Guid>
    {
        public GuidListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override Guid ConvertItem(object obj)
        {
            if (obj is Guid x) return x;
            return SimpleTypeConverters.ConvertToGuid(obj);
        }
    }
}