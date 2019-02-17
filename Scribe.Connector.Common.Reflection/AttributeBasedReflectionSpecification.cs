namespace Scribe.Connector.Common.Reflection
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    using Scribe.Core.ConnectorApi.Metadata;

    public class AttributeBasedReflectionSpecification : IReflectionSpecification
    {
        public Func<Type, bool> TypeFilter => TypeFilterImpl;

        public Func<Type, ObjDefStub> GetObjectDefFromType => GetObjectDefFromTypeImpl;

        private static bool TypeFilterImpl(Type t)
        {
            Debug.Assert(t != null, nameof(t) + " != null");
            return t.GetCustomAttributes<ObjectDefinitionAttribute>().Any();
        }

        private static ObjDefStub GetObjectDefFromTypeImpl(Type t)
        {
            Debug.Assert(t != null, nameof(t) + " != null");
            var objDef = t.GetCustomAttribute<ObjectDefinitionAttribute>();
            var name = string.IsNullOrWhiteSpace(objDef.Name) ? t.Name : objDef.Name.Trim();
            return new ObjDefStub(name, objDef.Description, objDef.Hidden);
        }
    }
}