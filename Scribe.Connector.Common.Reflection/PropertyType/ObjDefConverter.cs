using System.Net;

namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Scribe.Core.ConnectorApi;
    using Scribe.Core.ConnectorApi.Query;

    public class ObjDefs
    {
        public ObjDef GetOrBuild(string objName)
        {

            if (this.completed.TryGetValue(objName, out var objDef)) return objDef;
            if (this.headers.TryGetValue(objName, out var h))
            {
                this.InProgressOrComplete.Add(objName);
                var props = h.Properties.Values.Select(MakeFull);
                var od = new ObjDef(h.Name, h.Description, h.Hidden, h.Actions.Values, props);
                this.completed.Add(objName, od);

                return od;
            }

            return null;
        }

        private IFullProp MakeFull(IPropDef d)
        {
            if (d is IFullProp f) return f;

            if (d is DataEntityPropertyDef dePropDef)
            {

                var create = typeof(ObjDefs).GetMethod("CreateDEProp").MakeGenericMethod(dePropDef.DataEntityType);
                return (IFullProp)create.Invoke(null, new Object[] { dePropDef, this });
            }
            if (d is DataEntityListPropertyDef deListPropDef)
            {

                var create = typeof(ObjDefs).GetMethod("CreateDEList").MakeGenericMethod(deListPropDef.DataEntityType);
                return (IFullProp)create.Invoke(null, new Object[] { deListPropDef, this });
            }
            return null;
        }

        public static IFullProp CreateDEProp<T>(DataEntityPropertyDef d, ObjDefs e)
            where T : new()
        {
            return new DataEntityProperty<T>(d,e);
        }

        public static IFullProp CreateDEList<T>(DataEntityListPropertyDef d, ObjDefs e)
            where T : new()
        {
            return new DataEntityListProperty<T>(d, e);
        }

        private HashSet<string> InProgressOrComplete = new HashSet<string>();
        private Dictionary<string, ObjDefHeader> headers;

        private Dictionary<string, ObjDef> completed = new Dictionary<string, ObjDef>();

        public ObjDefs(IEnumerable<ObjDefHeader> headers)
        {
            this.headers = headers.ToDictionary(h => h.Name, h => h);
        }
    }

    public static class ObjDefConverter
    {
        public static T From<T>(DataEntity de, ObjDefs e) where T : new()
        {
            return From<T>(de, e, de.ObjectDefinitionFullName);
        }
        public static T From<T>(DataEntity de, ObjDefs e, string deType) where T : new()
        {
            var data = new T();
            var od = e.GetOrBuild(deType);
            foreach (var property in de.Properties)
            {
                var propertySetter = od.Properties[property.Key];
                propertySetter?.Set(data, property.Value);
            }

            foreach (var c in de.Children)
            {
                var dataEntityProp = od.Properties[c.Key] as IDataEntityProperty;
                if (dataEntityProp != null)
                {
                    var dataEntity = c.Value.FirstOrDefault();

                    if (dataEntity != null)
                    {
                        dataEntityProp.Set(data, dataEntity);
                    }
                }

                var dataEntityListProp = od.Properties[c.Key] as IDataEntityListProperty;
                if (dataEntityListProp != null)
                {
                    var dataEntity = c.Value;

                    if (dataEntity != null)
                    {
                        dataEntityListProp.Set(data, dataEntity);
                    }
                }

            }

            return data;
        }
        //public static T From<T>(DataEntity de, ObjDefs e) where T : new()
        //{
        //    var data = new T();
        //    // TODO: Consider getting this from the type instead of DataEntity
        //    var od = e.GetOrBuild(de.ObjectDefinitionFullName);
        //    foreach (var property in de.Properties)
        //    {
        //        var propertySetter = od.Properties[property.Key];
        //        propertySetter?.Set(data, property.Value);
        //    }

        //    foreach (var c in de.Children)
        //    {
        //        var dataEntityProp = od.Properties[c.Key] as IDataEntityProperty;
        //        if (dataEntityProp != null)
        //        {
        //            var dataEntity = c.Value.FirstOrDefault();

        //            if (dataEntity != null)
        //            {
        //                dataEntityProp.Set(data, dataEntity);
        //            }
        //        }

        //        var dataEntityListProp = od.Properties[c.Key] as IDataEntityListProperty;
        //        if (dataEntityListProp != null)
        //        {
        //            var dataEntity = c.Value;

        //            if (dataEntity != null)
        //            {
        //                dataEntityListProp.Set(data, dataEntity);
        //            }
        //        }

        //    }

        //    return data;
        //}

        public static DataEntity To<T>(T data, ObjDefs e, QueryEntity qe = null)
        {
            if (data == null) return null;
            var stub = MetadataReflector.GetObjectDefStubFromType(typeof(T));
            var od = e.GetOrBuild(stub.Name);
            var de = new DataEntity(od.Name);
            var props = new EntityProperties();
            var children = new EntityChildren();
            foreach (var keyValuePair in od.Properties)
            {
                var name = keyValuePair.Key;
                var getter = keyValuePair.Value;
                if (!getter.IsObjectDefProp)
                {
                    if (qe == null || qe.PropertyList.Contains(name))
                    {
                        props.Add(name, getter.Get(data));
                    }
                }
                else
                {
                    var dataEntityGetter = getter as IDataEntityProperty;
                    if (dataEntityGetter != null)
                    {

                        if (qe == null)
                        {
                            var cde = dataEntityGetter.Get(data);
                            var v = cde == null ? new List<DataEntity>() : new List<DataEntity> { cde };
                            children.Add(name, v);
                        }
                        else
                        {
                            var propQe = qe.ChildList.FirstOrDefault(q => q.Name == name);
                            if (propQe != null)
                            {
                                var cde = dataEntityGetter.Get(data, propQe);
                                var v = cde == null ? new List<DataEntity>() : new List<DataEntity> { cde };
                                children.Add(name, v);
                            }
                        }
                    }

                    var dataEntityListGetter = getter as IDataEntityListProperty;
                    if (dataEntityListGetter != null)
                    {
                        if (qe == null)
                        {
                            var cde = dataEntityListGetter.Get(data);
                            
                            // Workaround fopr CORE Bug
                            if (cde == null)
                            {
                                cde = new List<DataEntity>();
                            }
                            children.Add(name, cde);
                        }
                        else
                        {
                            var propQe = qe.ChildList.FirstOrDefault(q => q.Name == name);
                            if (propQe != null)
                            {
                                var cde = dataEntityListGetter.Get(data, propQe);
                                // Workaround fopr CORE Bug
                                if (cde == null)
                                {
                                    cde = new List<DataEntity>();
                                }
                                children.Add(name, cde);
                            }
                        }
                    }
                }
            }

            de.Properties = props;
            de.Children = children;
            return de;
        }
    }
}