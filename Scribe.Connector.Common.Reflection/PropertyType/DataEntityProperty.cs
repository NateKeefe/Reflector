using System.Diagnostics;

namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Scribe.Core.ConnectorApi;
    using Scribe.Core.ConnectorApi.Query;

    // This is the final pass
    public class DataEntityProperty<T> : DataEntityPropertyDef, IDataEntityProperty, IFullProp
        where T : new()
    {
        public DataEntityProperty(DataEntityPropertyDef deFirstPass, ObjDefs e)
            : base(deFirstPass, deFirstPass.Name, typeof(T), deFirstPass.getter, deFirstPass.setter)

        {
            this.GetQF = MakeQGetter(deFirstPass.getter, e, this);
            this.GetF = MakeGetter(deFirstPass.getter, e, this);
            this.SetF = MakeSetter(deFirstPass.setter, e, this, deFirstPass.PropertyType);
        }

        protected Action<object, DataEntity> SetF { get; set; }

        protected Func<object, DataEntity> GetF { get; set; }
        protected Func<object, QueryEntity, DataEntity> GetQF { get; set; }

        public DataEntity Get(object target)
        {
            return this.GetF(target);
        }

        public DataEntity Get(object target, QueryEntity queryEntity)
        {
            return this.GetQF(target, queryEntity);
        }

        public void Set(object target, DataEntity val)
        {
            this.SetF(target, val);
        }

        private static Action<object, DataEntity> MakeSetter(Action<object, object> setValue, ObjDefs objDefHeaders, ISettablePropertyDef context, string objDefName)
        {
            return (target, val) =>
                {
                    if (target == null) ThrowHelper.ThrowSetterTargetNull(context);
                    var tVal = ObjDefConverter.From<T>(val, objDefHeaders, objDefName);
                    setValue(target, tVal);
                };
        }

        private static Func<object, DataEntity> MakeGetter(Func<object, object> getValue, ObjDefs objDefHeader, ISettablePropertyDef context)
        {
            return target =>
                {
                    if (target == null) ThrowHelper.ThrowGetterTargetNull(context);
                    var val = getValue(target);
                    if (val == null) return null;
                    var de = ObjDefConverter.To((T)val, objDefHeader);
                    return de;
                };
        }

        private static Func<object, QueryEntity, DataEntity> MakeQGetter(Func<object, object> getValue, ObjDefs objDefHeader, ISettablePropertyDef context)
        {
            return (target, qe) =>
                {
                    if (target == null) ThrowHelper.ThrowGetterTargetNull(context);
                    var val = getValue(target);
                    if (val == null) return null;
                    var de = ObjDefConverter.To((T)val, objDefHeader, qe);
                    return de;
                };
        }

        object IProp.Get(object target)
        {
            return this.Get(target);
        }

        void IProp.Set(object target, object val)
        {
            this.Set(target, (DataEntity)val);
        }
    }

    public class DataEntityListProperty<T> : DataEntityListPropertyDef, IDataEntityListProperty, IFullProp
        where T : new()
    {
        public override bool IsObjectDefProp { get; } = true;

        public override bool IsCollection { get; } = true;


        public DataEntityListProperty(DataEntityListPropertyDef propertyDef, ObjDefs e)
            : base(propertyDef, propertyDef.Name, typeof(T), propertyDef.getter, propertyDef.setter, propertyDef.DotNetPropertyType)

        {
            this.GetF = MakeGetter(propertyDef.getter, e, this);
            this.GetQF = MakeQGetter(propertyDef.getter, e, this);
            this.SetF = MakeSetter(propertyDef.setter, e, this, propertyDef.DotNetPropertyType, propertyDef.PropertyType);
        }

        protected Func<object, QueryEntity, List<DataEntity>> GetQF { get; set; }

        protected Action<object, IEnumerable<DataEntity>> SetF { get; set; }

        protected Func<object, List<DataEntity>> GetF { get; set; }



        private static Action<object, IEnumerable<DataEntity>> MakeSetter(Action<object, object> setValue, ObjDefs objDefHeader, ISettablePropertyDef context, Type targetType, string deType)
        {
            return (target, val) =>
                {
                    if (target == null) ThrowHelper.ThrowSetterTargetNull(context);
                    if (val == null)
                    {
                        setValue(target, null);
                        return;
                    }
                    var tVal = val.Select(x => ObjDefConverter.From<T>(x, objDefHeader, deType));

                    if (targetType.IsArray)
                    {
                        setValue(target, tVal.ToArray());
                        return;
                    }

                    if (targetType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        setValue(target, tVal.ToList());
                        return;
                    }

                    setValue(target, tVal);
                    return;
                };
        }

        private static Func<object, List<DataEntity>> MakeGetter(Func<object, object> getValue, ObjDefs objDefHeader, ISettablePropertyDef context)
        {
            return target =>
                {
                    if (target == null) ThrowHelper.ThrowGetterTargetNull(context);
                    var val = getValue(target);
                    if (val == null) return null;
                    var ie = val as IEnumerable<T>;
                    var des = ie.Select(x => ObjDefConverter.To(x, objDefHeader));
                    return des.ToList();
                };
        }

        private static Func<object, QueryEntity, List<DataEntity>> MakeQGetter(Func<object, object> getValue, ObjDefs objDefHeader, ISettablePropertyDef context)
        {
            return (target, qe) =>
                {
                    if (target == null) ThrowHelper.ThrowGetterTargetNull(context);
                    var val = getValue(target);
                    if (val == null) return null;
                    var ie = val as IEnumerable<T>;
                    var des = ie.Select(x => ObjDefConverter.To(x, objDefHeader, qe));
                    return des.ToList();
                };
        }

        object IProp.Get(object target)
        {
            return this.Get(target);
        }

        public List<DataEntity> Get(object target, QueryEntity queryEntity)
        {
            return this.GetQF(target, queryEntity);
        }

        void IProp.Set(object target, object val)
        {
            this.Set(target, (IEnumerable<DataEntity>)val);
        }

        public List<DataEntity> Get(object target)
        {
            return this.GetF(target);
        }

        public void Set(object target, IEnumerable<DataEntity> val)
        {
            this.SetF(target, val);
        }
    }

}