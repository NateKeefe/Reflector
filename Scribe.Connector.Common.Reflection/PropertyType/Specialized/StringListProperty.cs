﻿namespace Scribe.Connector.Common.Reflection.PropertyType.Specialized
{
    using System;

    internal sealed class StringListProperty : SimpleListProperty<string>
    {
        public StringListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }

        protected override String ConvertItem(object obj)
        {
            if (obj is String x) return x;
            return SimpleTypeConverters.ConvertToString(obj);
        }
    }
}