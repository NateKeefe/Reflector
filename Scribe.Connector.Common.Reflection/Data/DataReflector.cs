namespace Scribe.Connector.Common.Reflection.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Scribe.Connector.Common.Reflection.PropertyType;
    using Scribe.Core.ConnectorApi;
    using Scribe.Core.ConnectorApi.Query;

    //using static Scribe.Connector.Common.Reflection.PropertyType.ObjDefConverter;

    public class Reflector
    {
        private readonly Dictionary<string, ObjDefHeader> objectDefHeaders;

        private readonly ObjDefs objDefsEnriched;

        private readonly IMetadata metadata;

        public Reflector(Type t) : this(Assembly.GetAssembly(t))
        {
            
        }
        public Reflector(Assembly assembly)
        {
            this.objectDefHeaders = MetadataReflector.GetAllObjectDefHeaders(assembly);
            this.objDefsEnriched = new ObjDefs(this.objectDefHeaders.Values);
            this.metadata = MetadataReflector.ReflectMetadata(this.objectDefHeaders);
        }

        public IMetadataProvider GetMetadataProvider()
        {
            return this.metadata.Wrap();
        }

        public DataEntity ToDataEntity<T>(T data, QueryEntity queryEntity)
        {
            return DataReflector.ToDataEntity(data, queryEntity, this.objDefsEnriched);
        }
        public IEnumerable<DataEntity> ToDataEntities<T>(IEnumerable<T> data, QueryEntity queryEntity)
        {
            return DataReflector.ToDataEntities(data, queryEntity, this.objDefsEnriched);
        }


        public DataEntity ToDataEntity<T>(T data)
        {
            return DataReflector.ToDataEntity(data, null, this.objDefsEnriched);
        }
        public IEnumerable<DataEntity> ToDataEntities<T>(IEnumerable<T> data)
        {
            return DataReflector.ToDataEntities(data, null, this.objDefsEnriched);
        }
        public T To<T>(DataEntity entity)
            where T : new()
        {
            return DataReflector.FromDataEntity<T>(entity, this.objDefsEnriched);
        }
    }

    internal static class DataReflector
    {

        /// <summary>
        ///     Publicly available method that converts an enumerable of a Type attributed with an ObjectDefinitionAttribute
        ///     into an enumerable DataEntities projected by the QueryEntity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public static IEnumerable<DataEntity> ToDataEntities<T>(
            IEnumerable<T> data,
            QueryEntity queryEntity,
            ObjDefs ods)
        {
            return data.Select(x => ObjDefConverter.To(x, ods, queryEntity));
        }

        /// <summary>
        ///     Publicly available method that converts a Type attributed with an ObjectDefinitionAttribute
        ///     into a DataEntity projected by the QueryEntity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public static DataEntity ToDataEntity<T>(T data, QueryEntity queryEntity, ObjDefs ods)
        {
            return ObjDefConverter.To(data, ods, queryEntity);
        }

        internal static DataEntity ToDataEntity<T>(T a, QueryEntity queryEntity = null)
        {
            var assembly = Assembly.GetAssembly(typeof(T));
            var m = MetadataReflector.GetAllObjectDefHeaders(assembly);
            var ods = new ObjDefs(m.Values);

            return ToDataEntity(a, queryEntity, ods);
        }

        internal static KeyValuePair<string, Func<object, object>> BuildGetter(PropertyInfo pi)
        {
            var candidate = pi.GetCustomAttribute<PropertyDefinitionAttribute>();
            var propDef = MetadataReflector.BuildByMemberInfo(pi, candidate);
            var prop = propDef as IFullProp;
            return new KeyValuePair<string, Func<object, object>>(propDef.Name, x => prop.Get(x));
        }

        public static T FromDataEntity<T>(DataEntity entity, ObjDefs objDefsEnriched)
            where T : new()
        {
            return ObjDefConverter.From<T>(entity, objDefsEnriched, entity.ObjectDefinitionFullName);
        }
    }
}