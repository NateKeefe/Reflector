namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using System;
    using Scribe.Connector.Common.Reflection;

    public abstract class SimplePropDefBase<T> : PropDefBase, IProp<T>, IFullProp
    {
        public SimplePropDefBase(ISettablePropertyDef pDef, string fieldName) : base(pDef, fieldName) { }

        protected Action<object, object> SetF { get; set; }

        protected Func<object, T> GetF { get; set; }

        public T Get(object target)
        {
            return this.GetF(target);
        }

        public void Set(object target, object val)
        {
            this.SetF(target, val);
        }

        object IProp.Get(object target)
        {
            return this.Get(target);
        }

        void IProp.Set(object target, object val)
        {
            this.Set(target, val);
        }

        protected abstract T Convert(object obj);
    }
}