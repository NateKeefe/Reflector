// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataReflector.cs" company="">
//   
// </copyright>
// <summary>
//   A metadata reflector.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    using Scribe.Connector.Common.Reflection.Actions;
    using Scribe.Connector.Common.Reflection.PropertyType;
    using Scribe.Connector.Common.Reflection.PropertyType.Specialized;
    using Scribe.Core.ConnectorApi;
    using Scribe.Core.ConnectorApi.Metadata;

    internal static class MetadataReflector
    {
        internal static Dictionary<string, ObjDefHeader> GetAllObjectDefHeaders(Assembly assembly)
        {
            var objDefTypes = GetObjectDefTypes(assembly).Select(BuildObjectDefHeader);

            return objDefTypes.ToDictionary(od => od.Name, od => od);
        }

        internal static ObjDefHeader BuildObjectDefHeader(Type ty)
        {

            var stub = GetObjectDefStubFromType(ty);
            var actions = ty.GetCustomAttributes<SupportedOperationAttribute>(true)
                .Select(a => a.ToActionDefinition())
                .ToDictionary(a => a.FullName, a => a);

            var query = ty.GetCustomAttributes<QueryAttribute>().Select(a => a.ToActionDefinition())
                .FirstOrDefault();

            if (query != null)
            {
                actions.Add(query.FullName, query);
            }

            var propertyCandidates = ty.GetMembers()
                .Where(m => m.GetCustomAttributes<PropertyDefinitionAttribute>().Any()).ToDictionary(
                    m => m,
                    m => m.GetCustomAttribute<PropertyDefinitionAttribute>());

            var props =
                propertyCandidates
                    .Select(propertyCandidate => BuildByMemberInfo(propertyCandidate.Key, propertyCandidate.Value))
                    .ToDictionary(p => p.Name, p => p);


            return new ObjDefHeader(stub.Name, stub.Description, stub.Hidden, actions, props);
        }

        internal static IPropDef BuildByMemberInfo(MemberInfo info, ISettablePropertyDef candidate)
        {
            string name = info.Name;
            Type ty;
            Func<object, object> getter;
            Action<object, object> setter;

            switch (info)
            {
                case PropertyInfo pi:
                    ty = pi.PropertyType;
                    getter = pi.GetValue;
                    setter = pi.SetValue;
                    return MakeTypedProperty(candidate, name, ty, getter, setter);
                case FieldInfo fi:
                    ty = fi.FieldType;
                    getter = fi.GetValue;
                    setter = fi.SetValue;
                    return MakeTypedProperty(candidate, name, ty, getter, setter);
                default:
                    ThrowHelper.ThrowNotSupportException($"Trying to build property definitions but that only works with Properties and Fields. This is a {info.MemberType}.");
                    return null;
            }
        }

        private static IPropDef MakeTypedProperty(ISettablePropertyDef propAttr, string name, Type ty, Func<object, object> getter, Action<object, object> setter)
        {
            if (ty.IsEnum) { return new EnumProperty(propAttr, name, ty, getter, setter); }

            // Boolean
            if (ty == typeof(Boolean)) return new BooleanProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Boolean?)) return new NBooleanProperty(propAttr, name, ty, getter, setter);

            if (ty == typeof(List<Boolean>)) return new BooleanListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Boolean>)) return new BooleanListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Boolean[])) return new BooleanListProperty(propAttr, name, ty, getter, setter);


            if (ty == typeof(List<Boolean?>)) return new NBooleanListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Boolean?>)) return new NBooleanListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Boolean?[])) return new NBooleanListProperty(propAttr, name, ty, getter, setter);


            // DateTime
            if (ty == typeof(DateTime)) return new DateTimeProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(DateTime?)) return new NDateTimeProperty(propAttr, name, ty, getter, setter);

            if (ty == typeof(List<DateTime>)) return new DateTimeListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<DateTime>)) return new DateTimeListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(DateTime[])) return new DateTimeListProperty(propAttr, name, ty, getter, setter);


            if (ty == typeof(List<DateTime?>)) return new NDateTimeListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<DateTime?>)) return new NDateTimeListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(DateTime?[])) return new NDateTimeListProperty(propAttr, name, ty, getter, setter);


            // Int32
            if (ty == typeof(Int32)) return new Int32Property(propAttr, name, ty, getter, setter);
            if (ty == typeof(Int32?)) return new NInt32Property(propAttr, name, ty, getter, setter);

            if (ty == typeof(List<Int32>)) return new Int32ListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Int32>)) return new Int32ListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Int32[])) return new Int32ListProperty(propAttr, name, ty, getter, setter);


            if (ty == typeof(List<Int32?>)) return new NInt32ListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Int32?>)) return new NInt32ListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Int32?[])) return new NInt32ListProperty(propAttr, name, ty, getter, setter);


            // Int16
            if (ty == typeof(Int16)) return new Int16Property(propAttr, name, ty, getter, setter);
            if (ty == typeof(Int16?)) return new NInt16Property(propAttr, name, ty, getter, setter);

            if (ty == typeof(List<Int16>)) return new Int16ListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Int16>)) return new Int16ListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Int16[])) return new Int16ListProperty(propAttr, name, ty, getter, setter);


            if (ty == typeof(List<Int16?>)) return new NInt16ListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Int16?>)) return new NInt16ListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Int16?[])) return new NInt16ListProperty(propAttr, name, ty, getter, setter);


            // Int64
            if (ty == typeof(Int64)) return new Int64Property(propAttr, name, ty, getter, setter);
            if (ty == typeof(Int64?)) return new NInt64Property(propAttr, name, ty, getter, setter);

            if (ty == typeof(List<Int64>)) return new Int64ListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Int64>)) return new Int64ListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Int64[])) return new Int64ListProperty(propAttr, name, ty, getter, setter);


            if (ty == typeof(List<Int64?>)) return new NInt64ListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Int64?>)) return new NInt64ListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Int64?[])) return new NInt64ListProperty(propAttr, name, ty, getter, setter);


            // Decimal
            if (ty == typeof(Decimal)) return new DecimalProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Decimal?)) return new NDecimalProperty(propAttr, name, ty, getter, setter);

            if (ty == typeof(List<Decimal>)) return new DecimalListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Decimal>)) return new DecimalListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Decimal[])) return new DecimalListProperty(propAttr, name, ty, getter, setter);


            if (ty == typeof(List<Decimal?>)) return new NDecimalListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Decimal?>)) return new NDecimalListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Decimal?[])) return new NDecimalListProperty(propAttr, name, ty, getter, setter);


            // Double
            if (ty == typeof(Double)) return new DoubleProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Double?)) return new NDoubleProperty(propAttr, name, ty, getter, setter);

            if (ty == typeof(List<Double>)) return new DoubleListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Double>)) return new DoubleListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Double[])) return new DoubleListProperty(propAttr, name, ty, getter, setter);


            if (ty == typeof(List<Double?>)) return new NDoubleListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Double?>)) return new NDoubleListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Double?[])) return new NDoubleListProperty(propAttr, name, ty, getter, setter);


            // Single
            if (ty == typeof(Single)) return new SingleProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Single?)) return new NSingleProperty(propAttr, name, ty, getter, setter);

            if (ty == typeof(List<Single>)) return new SingleListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Single>)) return new SingleListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Single[])) return new SingleListProperty(propAttr, name, ty, getter, setter);


            if (ty == typeof(List<Single?>)) return new NSingleListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Single?>)) return new NSingleListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Single?[])) return new NSingleListProperty(propAttr, name, ty, getter, setter);


            // Guid
            if (ty == typeof(Guid)) return new GuidProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Guid?)) return new NGuidProperty(propAttr, name, ty, getter, setter);

            if (ty == typeof(List<Guid>)) return new GuidListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Guid>)) return new GuidListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Guid[])) return new GuidListProperty(propAttr, name, ty, getter, setter);


            if (ty == typeof(List<Guid?>)) return new NGuidListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Guid?>)) return new NGuidListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Guid?[])) return new NGuidListProperty(propAttr, name, ty, getter, setter);


            // String
            if (ty == typeof(String)) return new StringProperty(propAttr, name, ty, getter, setter);

            if (ty == typeof(List<String>)) return new StringListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<String>)) return new StringListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(String[])) return new StringListProperty(propAttr, name, ty, getter, setter);

            // Char
            if (ty == typeof(Char)) return new CharProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Char?)) return new NCharProperty(propAttr, name, ty, getter, setter);

            if (ty == typeof(List<Char>)) return new CharListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Char>)) return new CharListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Char[])) return new CharListProperty(propAttr, name, ty, getter, setter);


            if (ty == typeof(List<Char?>)) return new NCharListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Char?>)) return new NCharListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Char?[])) return new NCharListProperty(propAttr, name, ty, getter, setter);


            // Byte
            if (ty == typeof(Byte[])) return new ByteArrayProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Byte)) return new ByteProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Byte?)) return new NByteProperty(propAttr, name, ty, getter, setter);

            if (ty == typeof(List<Byte>)) return new ByteListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Byte>)) return new ByteListProperty(propAttr, name, ty, getter, setter);
            //if (ty == typeof(Byte[])) return new ByteListProperty(propAttr, name, ty, getter, setter);

            if (ty == typeof(List<Byte?>)) return new NByteListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(IEnumerable<Byte?>)) return new NByteListProperty(propAttr, name, ty, getter, setter);
            if (ty == typeof(Byte?[])) return new NByteListProperty(propAttr, name, ty, getter, setter);


            // Check if it is an Object Defintion or some type of list of object definition

            if (ty.GetCustomAttributes<ObjectDefinitionAttribute>().Any())
            {
                return new DataEntityPropertyDef(propAttr, name, ty, getter, setter);
            }

            if (ty.IsArray)
            {
                var underTy = ty.GetElementType();
                return new DataEntityListPropertyDef(propAttr, name, underTy, getter, setter, ty);
            }

            if (ty.GetGenericTypeDefinition() == typeof(List<>))
            {
                var underlying =  ty.GetGenericArguments()[0];
                return new DataEntityListPropertyDef(propAttr, name, underlying, getter, setter, ty);
            }

            if (ty.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var underlying = ty.GetGenericArguments()[0];
                return new DataEntityListPropertyDef(propAttr, name, underlying, getter, setter, ty);
            }





            throw new NotImplementedException($"We do not yet have support for lists of hierarchical properties. Property: {name},  Type: {ty.FullName}");
        }

        internal static IEnumerable<Type> GetObjectDefTypes(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.GetCustomAttributes<ObjectDefinitionAttribute>().Any());
        }

        internal static ObjDefStub GetObjectDefStubFromType(Type t)
        {
            Debug.Assert(t != null, nameof(t) + " != null");
            var objDef = t.GetCustomAttribute<ObjectDefinitionAttribute>();
            var name = string.IsNullOrWhiteSpace(objDef.Name) ? t.Name : objDef.Name.Trim();
            return new ObjDefStub(name, objDef.Description, objDef.Hidden);
        }

        public static IMetadata ReflectMetadata(Type t)
        {
            var assembly = Assembly.GetAssembly(t);

            var objDefs = GetAllObjectDefHeaders(assembly);
            // Need to separate the ObjDefHead from Full ObjDef

            var actions = objDefs.Values
                .SelectMany(od => od.Actions)
                .ToLookup(kv => kv.Key, kv => kv.Value)
                .ToDictionary(k => k.Key, k => k.Aggregate(MergeActions) as IActionDefinition);

            var objects = objDefs.ToDictionary(kv => kv.Key, kv => ObjDefHeader.ToObjectDefinition(kv.Value));

            return new Metadata(new ReadOnlyDictionary<string, IActionDefinition>(actions), new ReadOnlyDictionary<string, IObjectDefinition>(objects));
        }

        public static IMetadata ReflectMetadata(Dictionary<string, ObjDefHeader> objDefs)
        {
            var actions = objDefs.Values
                .SelectMany(od => od.Actions)
                .ToLookup(kv => kv.Key, kv => kv.Value)
                .ToDictionary(k => k.Key, k => k.Aggregate(MergeActions) as IActionDefinition);

            var objects = objDefs.ToDictionary(kv => kv.Key, kv => ObjDefHeader.ToObjectDefinition(kv.Value));

            return new Metadata(new ReadOnlyDictionary<string, IActionDefinition>(actions), new ReadOnlyDictionary<string, IObjectDefinition>(objects));
        }

        internal static IObjectDefinition Define<T>()
        {
            var stub = GetObjectDefStubFromType(typeof(T));
            var assembly = Assembly.GetAssembly(typeof(T));

            var objDefs = GetAllObjectDefHeaders(assembly);

            return ObjDefHeader.ToObjectDefinition(objDefs[stub.Name]);
        }

        public static IMetadataProvider Wrap(this IMetadata metadata)
        {
            return new MetadataWrapper(metadata);
        }

        private static ActionDef MergeActions(ActionDef a, ActionDef b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));

            if (a.FullName != b.FullName)
            {
                throw new NotSupportedException(
                    $"Merging actions is expected only on actions with the same FullName. In this case trying to merge '{a.FullName}' and '{b.FullName}'.");
            }
            if (a.KnownActionType != b.KnownActionType)
                throw new NotSupportedException(
                    $"There are two actions named {a.FullName}, but with different known actions: '{a.KnownActionType}' and '{b.KnownActionType}'.");

            switch (a.KnownActionType)
            {
                case KnownActions.Create:
                    return new CreateAction(a.FullName, a.Description, a.SupportsBulk || b.SupportsBulk);
                case KnownActions.UpdateInsert:
                    return new UpsertAction(a.FullName, a.Description, a.SupportsBulk || b.SupportsBulk);
                case KnownActions.Query:
                    return new QueryAction();
                case KnownActions.Update:
                    return new UpdateAction(
                        a.FullName,
                        a.Description,
                        a.SupportsBulk || b.SupportsBulk,
                        a.SupportsMultipleRecordOperations || b.SupportsMultipleRecordOperations);
                case KnownActions.Delete:
                    return new DeleteAction(
                        a.FullName,
                        a.Description,
                        a.SupportsBulk || b.SupportsBulk,
                        a.SupportsMultipleRecordOperations || b.SupportsMultipleRecordOperations);
                case KnownActions.NativeQuery:
                    return new NativeQueryAction();
                case KnownActions.CreateWith:
                    return new CreateWithAction(a.FullName, a.Description, a.SupportsBulk || b.SupportsBulk);
                case KnownActions.UpdateWith:
                    return new UpdateWithAction(
                        a.FullName,
                        a.Description,
                        a.SupportsBulk || b.SupportsBulk,
                        a.SupportsMultipleRecordOperations || b.SupportsMultipleRecordOperations);
                case KnownActions.None:
                case KnownActions.InsertUpdate:
                default:
                    throw new NotSupportedException(
                        $"Metadata reflection does not support {a.KnownActionType}");
            }
            throw new NotSupportedException(
                $"There are two actions named {a.FullName}, but with different known actions: '{a.KnownActionType}' and '{b.KnownActionType}'.");
        }
    }

    /// <summary>A metadata reflector.</summary>
    //public static class MetadataReflector
    //{
    //    /// <summary>The define.</summary>
    //    /// <param name="t">The type to reflect over. This type should correspond to an Object Definition.</param>
    //    /// <param name="spec">The specification for how to reflect.</param>
    //    /// <returns>The <see cref="IObjectDefinition"/>.</returns>
    //    /// <exception cref="ArgumentNullException"><paramref name="t"/> is <see langword="null"/> or <paramref name="spec"/> is <see langword="null"/></exception>
    //    internal static IObjectDefinition Define(Type t, IReflectionSpecification spec)
    //    {
    //        if (t == null) throw new ArgumentNullException(nameof(t));
    //        if (spec == null) throw new ArgumentNullException(nameof(spec));
    //        var top = BuildObjectDefHeader(t, spec.GetObjectDefFromType(t));
    //        return new TopLevelObjDef(top);
    //    }


    //    internal static IObjectDefinition Define<T>()
    //    {
    //        var ty = typeof(T);
    //        return Define(ty, new AttributeBasedReflectionSpecification());
    //    }

    //    internal static ObjDefHeader DefineXXX<T>()
    //    {
    //        var t = typeof(T);
    //        var spec = new AttributeBasedReflectionSpecification();
    //        if (t == null) throw new ArgumentNullException(nameof(t));
    //        if (spec == null) throw new ArgumentNullException(nameof(spec));
    //        return BuildObjectDefHeader(t, spec.GetObjectDefFromType(t));
    //    }

    //    public static IMetadata ReflectMetadata(IEnumerable<Type> types)
    //    {

    //        // Need to separate the ObjDefHead from Full ObjDef
    //        var objDefs = DefineManyImpl(types, new AttributeBasedReflectionSpecification());

    //        var actions = objDefs
    //            .SelectMany(od => od.Actions)
    //            .ToLookup(kv => kv.Key, kv => kv.Value)
    //            .ToDictionary(k => k.Key, k => k.Aggregate(MergeActions));

    //        var objects = objDefs.Cast<IObjectDefinition>().ToDictionary(kv => kv.FullName, kv => kv);

    //        return new Metadata(new ReadOnlyDictionary<string, IActionDefinition>(actions), new ReadOnlyDictionary<string, IObjectDefinition>(objects));
    //    }

    //    public static IMetadataProvider Wrap(this IMetadata metadata)
    //    {
    //        return new MetadataWrapper(metadata);
    //    }

    //    private static IActionDefinition MergeActions(IActionDefinition a, IActionDefinition b)
    //    {
    //        if (a == null) throw new ArgumentNullException(nameof(a));
    //        if (b == null) throw new ArgumentNullException(nameof(b));

    //        if (a.FullName != b.FullName)
    //        {
    //            throw new NotSupportedException(
    //                $"Merging actions is expected only on actions with the same FullName. In this case trying to merge '{a.FullName}' and '{b.FullName}'.");
    //        }
    //        if (a.KnownActionType != b.KnownActionType)
    //            throw new NotSupportedException(
    //                $"There are two actions named {a.FullName}, but with different known actions: '{a.KnownActionType}' and '{b.KnownActionType}'.");

    //        switch (a.KnownActionType)
    //        {
    //            case KnownActions.Create:
    //                return new CreateAction(a.FullName, a.Description, a.SupportsBulk || b.SupportsBulk);
    //            case KnownActions.UpdateInsert:
    //                return new UpsertAction(a.FullName, a.Description, a.SupportsBulk || b.SupportsBulk);
    //            case KnownActions.Query:
    //                return new QueryAction();
    //            case KnownActions.Update:
    //                return new UpdateAction(
    //                    a.FullName,
    //                    a.Description,
    //                    a.SupportsBulk || b.SupportsBulk,
    //                    a.SupportsMultipleRecordOperations || b.SupportsMultipleRecordOperations);
    //            case KnownActions.Delete:
    //                return new DeleteAction(
    //                    a.FullName,
    //                    a.Description,
    //                    a.SupportsBulk || b.SupportsBulk,
    //                    a.SupportsMultipleRecordOperations || b.SupportsMultipleRecordOperations);
    //            case KnownActions.NativeQuery:
    //                return new NativeQueryAction();
    //            case KnownActions.CreateWith:
    //                return new CreateWithAction(a.FullName, a.Description, a.SupportsBulk || b.SupportsBulk);
    //            case KnownActions.UpdateWith:
    //                return new UpdateWithAction(
    //                    a.FullName,
    //                    a.Description,
    //                    a.SupportsBulk || b.SupportsBulk,
    //                    a.SupportsMultipleRecordOperations || b.SupportsMultipleRecordOperations);
    //            case KnownActions.None:
    //            case KnownActions.InsertUpdate:
    //            default:
    //                throw new NotSupportedException(
    //                    $"Metadata reflection does not support {a.KnownActionType}");
    //        }
    //        throw new NotSupportedException(
    //            $"There are two actions named {a.FullName}, but with different known actions: '{a.KnownActionType}' and '{b.KnownActionType}'.");
    //    }

    //    internal static IEnumerable<IObjectDefinition> DefineMany(IEnumerable<Type> types)
    //    {
    //        return DefineMany(types, new AttributeBasedReflectionSpecification());
    //    }

    //    internal static IEnumerable<IObjectDefinition> DefineMany(IEnumerable<Type> types, IReflectionSpecification spec)
    //    {
    //        return DefineManyImpl(types, spec);
    //    }

    //    private static IEnumerable<ObjDef> DefineManyImpl(IEnumerable<Type> types, IReflectionSpecification spec)
    //    {
    //        var memoized = new ConcurrentDictionary<string, ObjDef>();

    //        var objDefDictionary = types.Where(spec.TypeFilter).ToDictionary(t => t, spec.GetObjectDefFromType);

    //        foreach (var candidate in objDefDictionary)
    //        {
    //            var objDef = BuildObjectDef(candidate.Key, candidate.Value);
    //            yield return objDef;
    //        }
    //    }

    //    private static ObjDefHeader BuildObjectDefHeader(Type ty, ObjDefStub stub)
    //    {
    //        var actions = ty.GetCustomAttributes<SupportedOperationAttribute>(true)
    //            .Select(a => a.ToActionDefinition())
    //            .ToDictionary(a => a.FullName, a => a);

    //        var query = ty.GetCustomAttributes<QueryAttribute>().Select(a => a.ToActionDefinition())
    //            .FirstOrDefault();

    //        if (query != null)
    //        {
    //            actions.Add(query.FullName, query);
    //        }

    //        var propertyCandidates = ty.GetMembers()
    //            .Where(m => m.GetCustomAttributes<PropertyDefinitionAttribute>().Any()).ToDictionary(
    //                m => m,
    //                m => m.GetCustomAttribute<PropertyDefinitionAttribute>());

    //        var props = 
    //            propertyCandidates
    //            .Select(propertyCandidate => BuildByMemberInfo(propertyCandidate.Key, propertyCandidate.Value))
    //            .ToDictionary(p => p.Name, p => p);


    //        return new ObjDefHeader(stub.Name, stub.Description, stub.Hidden, actions, props);
    //    }

    //    private static ObjDef BuildObjectDef(Type ty, ObjDefStub stub)
    //    {
    //        var actions = ty.GetCustomAttributes<SupportedOperationAttribute>(true)
    //            .Select(a => a.ToActionDefinition()).ToList();

    //        var query = ty.GetCustomAttributes<QueryAttribute>().Select(a => a.ToActionDefinition())
    //            .FirstOrDefault();

    //        if (query != null)
    //        {
    //            actions.Add(query);
    //        }

    //        var propertyCandidates = ty.GetMembers()
    //            .Where(m => m.GetCustomAttributes<PropertyDefinitionAttribute>().Any()).ToDictionary(
    //                m => m,
    //                m => m.GetCustomAttribute<PropertyDefinitionAttribute>());

    //        var props = propertyCandidates.Select(
    //            propertyCandidate => BuildByMemberInfo(propertyCandidate.Key, propertyCandidate.Value)).ToList();

    //        var objDef = new ObjDef(stub.Name, stub.Description, stub.Hidden, actions, props);
    //        return objDef;
    //    }


    //    private static IPropDef BuildByMemberInfo(MemberInfo info, ISettablePropertyDef candidate)
    //    {
    //        string name = info.Name;
    //        Type ty;
    //        Func<object, object> getter;
    //        Action<object, object> setter;

    //        switch (info)
    //        {
    //            case PropertyInfo pi:
    //                ty = pi.PropertyType;
    //                getter = pi.GetValue;
    //                setter = pi.SetValue;
    //                return MakeTypedProperty(candidate, name, ty, getter, setter);
    //            case FieldInfo fi:
    //                ty = fi.FieldType;
    //                getter = fi.GetValue;
    //                setter = fi.SetValue;
    //                return MakeTypedProperty(candidate, name, ty, getter, setter);
    //            default:
    //                ThrowHelper.ThrowNotSupportException($"Trying to build property definitions but that only works with Properties and Fields. This is a {info.MemberType}.");
    //                return null;
    //        }
    //    }

    //    private static IPropDef MakeTypedProperty(ISettablePropertyDef propAttr, string name, Type ty, Func<object, object> getter, Action<object, object> setter)
    //    {
    //        // Boolean
    //        if (ty == typeof(Boolean)) return new BooleanProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Boolean?)) return new NBooleanProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Boolean>)) return new BooleanListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(IEnumerable<Boolean>)) return new BooleanListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Boolean[])) return new BooleanListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Boolean?[])) return new NBooleanListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Boolean?>)) return new NBooleanListProperty(propAttr, name, ty, getter, setter);

    //        // DateTime
    //        if (ty == typeof(DateTime)) return new DateTimeProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(DateTime?)) return new NDateTimeProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<DateTime>)) return new DateTimeListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(IEnumerable<DateTime>)) return new DateTimeListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(DateTime[])) return new DateTimeListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(DateTime?[])) return new NDateTimeListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<DateTime?>)) return new NDateTimeListProperty(propAttr, name, ty, getter, setter);

    //        // Int32
    //        if (ty == typeof(Int32)) return new Int32Property(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Int32?)) return new NInt32Property(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Int32>)) return new Int32ListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(IEnumerable<Int32>)) return new Int32ListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Int32[])) return new Int32ListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Int32?[])) return new NInt32ListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Int32?>)) return new NInt32ListProperty(propAttr, name, ty, getter, setter);

    //        // Int16
    //        if (ty == typeof(Int16)) return new Int16Property(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Int16?)) return new NInt16Property(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Int16>)) return new Int16ListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(IEnumerable<Int16>)) return new Int16ListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Int16[])) return new Int16ListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Int16?[])) return new NInt16ListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Int16?>)) return new NInt16ListProperty(propAttr, name, ty, getter, setter);

    //        // Int64
    //        if (ty == typeof(Int64)) return new Int64Property(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Int64?)) return new NInt64Property(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Int64>)) return new Int64ListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(IEnumerable<Int64>)) return new Int64ListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Int64[])) return new Int64ListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Int64?[])) return new NInt64ListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Int64?>)) return new NInt64ListProperty(propAttr, name, ty, getter, setter);

    //        // Decimal
    //        if (ty == typeof(Decimal)) return new DecimalProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Decimal?)) return new NDecimalProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Decimal>)) return new DecimalListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(IEnumerable<Decimal>)) return new DecimalListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Decimal[])) return new DecimalListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Decimal?[])) return new NDecimalListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Decimal?>)) return new NDecimalListProperty(propAttr, name, ty, getter, setter);

    //        // Double
    //        if (ty == typeof(Double)) return new DoubleProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Double?)) return new NDoubleProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Double>)) return new DoubleListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(IEnumerable<Double>)) return new DoubleListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Double[])) return new DoubleListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Double?[])) return new NDoubleListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Double?>)) return new NDoubleListProperty(propAttr, name, ty, getter, setter);

    //        // Single
    //        if (ty == typeof(Single)) return new SingleProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Single?)) return new NSingleProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Single>)) return new SingleListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(IEnumerable<Single>)) return new SingleListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Single[])) return new SingleListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Single?[])) return new NSingleListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Single?>)) return new NSingleListProperty(propAttr, name, ty, getter, setter);

    //        // Guid
    //        if (ty == typeof(Guid)) return new GuidProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Guid?)) return new NGuidProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Guid>)) return new GuidListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(IEnumerable<Guid>)) return new GuidListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Guid[])) return new GuidListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Guid?[])) return new NGuidListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Guid?>)) return new NGuidListProperty(propAttr, name, ty, getter, setter);

    //        // String
    //        if (ty == typeof(String)) return new StringProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<String>)) return new StringListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(IEnumerable<String>)) return new StringListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(String[])) return new StringListProperty(propAttr, name, ty, getter, setter);

    //        // Char
    //        if (ty == typeof(Char)) return new CharProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Char?)) return new NCharProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Char>)) return new CharListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(IEnumerable<Char>)) return new CharListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Char[])) return new CharListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Char?[])) return new NCharListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Char?>)) return new NCharListProperty(propAttr, name, ty, getter, setter);

    //        // Byte
    //        if (ty == typeof(Byte)) return new ByteProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Byte?)) return new NByteProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Byte>)) return new ByteListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(IEnumerable<Byte>)) return new ByteListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Byte[])) return new ByteListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(Byte?[])) return new NByteListProperty(propAttr, name, ty, getter, setter);
    //        if (ty == typeof(List<Byte?>)) return new NByteListProperty(propAttr, name, ty, getter, setter);

    //        // Check if it is an Object Defintion or some type of list of object definition

    //        if (ty.GetCustomAttributes<ObjectDefinitionAttribute>().Any())
    //        {
    //            return new DataEntityPropertyDef(propAttr, name, ty, getter, setter);
    //        }

    //        //if (ty.IsArray || ty.)
    //        //{

    //        //}




    //        throw new NotImplementedException("We do not yet have support for hierarchical properties.");
    //    }

    //    public static DataEntityProperty<T> Finish<T>(DataEntityPropertyDef dataEntityPropertyDef, ObjDefHeader od)
    //        where T : new()
    //    {
    //        return new DataEntityProperty<T>(dataEntityPropertyDef, od);
    //    }

    //    private static void MetaReflStuff()
    //    {
    //        //var createF = typeof(MetadataReflector).GetMethod("CreateDataEntityProperty").MakeGenericMethod(ty);
    //        //var attr = ty.GetCustomAttribute<ObjectDefinitionAttribute>();
    //        //var header = new ObjDefHeader();

    //        //var x = (IPropDef)createF.Invoke(null, new object[] { propAttr, header, name, ty });
    //        //return x;            
    //    }

    //    private static MethodInfo __ReflectionHelper_CreateDataEntity =
    //        typeof(MetadataReflector).GetMethod("CreateDataEntityProperty", BindingFlags.Static);


    //}
}