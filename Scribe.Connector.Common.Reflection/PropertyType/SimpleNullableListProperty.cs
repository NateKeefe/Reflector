namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using System;

    /// <summary>
    ///  Valid Lists are:
    /// IEnumerable<T>
    /// List<T>
    /// int[], string[], dataE[], etc.
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public abstract class SimpleNullableListProperty<T> : SimpleListProperty<T?> where T : struct
    {
        // This is necessary for the nullable lists to support the correct property type
        public override string PropertyType { get; } = typeof(T).FullName;

        protected SimpleNullableListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, propType, getter, setter)
        {
        }
    }
}