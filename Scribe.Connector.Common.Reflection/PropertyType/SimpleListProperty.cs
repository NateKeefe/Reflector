namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Scribe.Connector.Common.Reflection;

    public abstract class SimpleListProperty<T> : SimplePropDefBase<List<T>>
    {
        public override bool IsObjectDefProp { get; } = false;

        private static Action<object, object> MakeSetter(Action<object, object> setValue, ISettablePropertyDef context, Func<object, List<T>> convert, bool isArray)
        {
            return (target, val) =>
                {
                    if (target == null) ThrowHelper.ThrowGetterTargetNull(context);
                    var tVal = convert(val);
                    if (target == null) ThrowHelper.ThrowSetterTargetNull(context);
                    setValue(target, isArray ? (object)tVal?.ToArray() : tVal);
                };
        }

        private static Func<object, List<T>> MakeGetter(Func<object, object> getValue, ISettablePropertyDef context, Func<object, List<T>> convert)
        {
            return target =>
                {
                    if (target == null) ThrowHelper.ThrowGetterTargetNull(context);
                    var val = getValue(target);
                    return convert(val);
                };
        }

        protected SimpleListProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName)
        {
            this.GetF = MakeGetter(getter, this, this.Convert);
            this.SetF = MakeSetter(setter, this, this.Convert, propType.IsArray);
        }

        protected abstract T ConvertItem(object obj);

        protected override List<T> Convert(object obj)
        {
            if (obj is null) return null;
            if (obj is List<T> ls) return ls;
            if (obj is IEnumerable<T> ie) return ie.ToList();

            if (obj is IEnumerable<object> ieo) return ieo.Select(ConvertItem).ToList();

            ThrowHelper.ThrowTypesDoNotMatch(obj.GetType(), typeof(IEnumerable<T>), this);
            return null;
        }

        public override string PropertyType { get; } = typeof(T).FullName;

        public override bool IsCollection { get; } = true;
    }
}