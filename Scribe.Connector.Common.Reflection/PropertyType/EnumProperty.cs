using System;
using System.Collections.Generic;
using System.Linq;

namespace Scribe.Connector.Common.Reflection.PropertyType {
    public class EnumProperty : PropDefBase, IFullProp
    {
        public static string BuildDescription(Type enumType, string previousDescription)
        {
            var labelsAndIndex = Enum.GetNames(enumType).Select(str => GetLabelAndIndex(str, enumType)).Select(kv => $"{kv.Key}({kv.Value})");
            var generatedDescription =
                $"This property is based on the enum type {enumType.Name}. Defined values are {string.Join(", ", labelsAndIndex)}.";
            
            if (String.IsNullOrWhiteSpace(previousDescription))
            {
                return generatedDescription;
            }

            var trimmed = previousDescription.Trim();
            var hasPeriod = trimmed.EndsWith(".");
            var spacer = hasPeriod ? " " : ". ";
            var combined = trimmed + spacer + generatedDescription;
            return combined;
        }

        public static KeyValuePair<string, object> GetLabelAndIndex(string name, Type enumType)
        {
            var x = (Enum)Enum.Parse(enumType, name);
            var i = System.Convert.ChangeType(x, x.GetTypeCode());
            return new KeyValuePair<string, object>(name, i);
        }
    
        public EnumProperty(ISettablePropertyDef pDef, string propName, Type propType, Func<object, object> getter, Action<object, object> setter)
            : base(pDef, propName, BuildDescription(propType, pDef.Description))
        {
            this.expectedType = propType;
            this.GetF = MakeGetter(getter, this, this.Convert);
            this.SetF = MakeSetter(setter, this, this.Convert);
            
        }

        private static Action<object, object> MakeSetter(Action<object, object> setValue, ISettablePropertyDef context, Func<object, Enum> convert)
        {
            return (target, val) =>
            {
                var tVal = convert(val);
                if (target == null) ThrowHelper.ThrowSetterTargetNull(context);
                setValue(target, tVal);
            };
        }

        private static Func<object, Enum> MakeGetter(Func<object, object> getValue, ISettablePropertyDef context, Func<object, Enum> convert)
        {
            return target =>
            {
                if (target == null) ThrowHelper.ThrowGetterTargetNull(context);
                var val = getValue(target);
                if (val is Enum e) return e;
                return convert(val);
            };
        }

        protected Enum Convert(object obj)
        {
            return EnumConverters.ConvertToEnum(this.expectedType, obj);
        }

        private readonly Type expectedType;
        protected Action<object, object> SetF { get; set; }

        protected Func<object, Enum> GetF { get; set; }


        public void Set(object target, object val)
        {
            this.SetF(target, val);
        }

        object IProp.Get(object target)
        {
            return this.GetF(target).ToString();
        }

        void IProp.Set(object target, object val)
        {
            this.Set(target, val);
        }

        public override bool IsObjectDefProp { get; } = false;
        public override string PropertyType { get; } = "System.String";



    }
}