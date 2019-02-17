namespace Scribe.Connector.Common.Reflection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionExtensions
    {
        public static bool IsNullable(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsEnumerable(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
        }

        public static (bool, Type) IsEnumerableT(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (type == typeof(string)) return (false, type);
            if (type.IsArray) return (true, type.GetElementType());

            var enumberableI = type.Name == "IEnumerable`1"
                                    ? type
                                    : type.GetInterface(typeof(IEnumerable<>).FullName);
            if (enumberableI == null) return (false, type);

            return (true, enumberableI.GenericTypeArguments[0]);
        }

        public static bool IsFromMscorlib(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.Assembly == typeof(int).Assembly;
        }

        public static bool IsSimpleType(this Type type)
        {
            return SimpleTypes.Types.Contains(type);
        }

        

        public static Type GetPropertyDefinitionType(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.IsArray
                       ? type.GetElementType().IsNullable()
                             ? type.GetElementType().GenericTypeArguments.First()
                             : type.GetElementType()
                       : type.IsNullable()
                           ? type.GenericTypeArguments.First()
                           : type.IsEnumerable()
                               ? type.GenericTypeArguments.Any()
                                     ? type.GenericTypeArguments.First().IsNullable()
                                           ? type.GenericTypeArguments.First().GenericTypeArguments.First()
                                           : type.GenericTypeArguments.First()
                                     : typeof(object)
                               : type;
        }

        public static string GetTypeName(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return IsNullable(type) ? type.GenericTypeArguments.First().FullName : type.FullName;
        }

        public static string GetObjectDefinitionFullName<T>()
        {
            return typeof(T).GetObjectDefinitionFullName();
        }

        public static string GetObjectDefinitionFullName(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var definitions = type.GetCustomAttributes<ObjectDefinitionAttribute>().ToList();

            if (!definitions.Any())
                throw new ArgumentException($"Did not find a type named {type.FullName}.", nameof(type));

            return string.IsNullOrWhiteSpace(definitions.Single().Name) ? type.Name : definitions.Single().Name.Trim();
        }

        //public static string GetPropertyDefinitionFullName<T>(string propertyName)
        //{
        //    return typeof(T).GetPropertyDefinitionFullName(propertyName);
        //}

        //public static string GetPropertyDefinitionFullName(this Type type, string propertyName)
        //{
        //    if (type == null) throw new ArgumentNullException(nameof(type));
        //    if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        //    var definitions = type.GetMembers().Where(m => m.Name == propertyName)
        //        .SelectMany(m => m.GetCustomAttributes<PropertyDefinitionAttribute>())
        //        .ToList();

        //    if (!definitions.Any())
        //        throw new ArgumentException(
        //            $"There was no property {propertyName} found on type {type.FullName}.",
        //            nameof(propertyName));

        //    return string.IsNullOrWhiteSpace(definitions.Single().Name)
        //               ? propertyName
        //               : definitions.Single().Name.Trim();
        //}
    }
}