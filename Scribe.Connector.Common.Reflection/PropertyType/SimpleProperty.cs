namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using System;
    using Scribe.Connector.Common.Reflection;

    public abstract class SimpleProperty<T> : SimplePropDefBase<T>
    {
        protected virtual Type ExpectedType { get; } = typeof(T);

        public SimpleProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName)
        {
            if (propType != this.ExpectedType) ThrowHelper.ThrowTypesDoNotMatch(propType, this.ExpectedType, pDef);

            this.GetF = MakeGetter(getter, this, this.Convert);
            this.SetF = MakeSetter(setter, this, this.Convert);
        }

        private static Action<object, object> MakeSetter(Action<object, object> setValue, ISettablePropertyDef context, Func<object, T> convert)
        {
            return (target, val) =>
                {
                    var tVal = convert(val);
                    if (target == null) ThrowHelper.ThrowSetterTargetNull(context);
                    setValue(target, tVal);
                };
        }

        private static Func<object, T> MakeGetter(Func<object, object> getValue, ISettablePropertyDef context, Func<object, T> convert)
        {
            return target =>
                {
                    if (target == null) ThrowHelper.ThrowGetterTargetNull(context);
                    var val = getValue(target);
                    return convert(val);
                };
        }

        public override string PropertyType { get; } = typeof(T).FullName;

        public override bool IsObjectDefProp { get; } = false;

        public override bool IsCollection { get; } = false;

    }
}