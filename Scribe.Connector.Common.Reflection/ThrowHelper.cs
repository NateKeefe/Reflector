using System.Collections.Generic;

namespace Scribe.Connector.Common.Reflection
{
    using System;

    internal class ThrowHelper
    {
        internal static void ThrowNotSupportException() { throw new NotSupportedException(); }

        internal static void ThrowNotSupportException(string message) { throw new NotSupportedException(message); }

        internal static void ThrowNotSupportException(string message, Exception inner)
        {
            throw new NotSupportedException(message, inner);
        }

        internal static void ThrowInvalidOperationException() { throw new InvalidOperationException(); }

        internal static void ThrowInvalidOperationException(string message)
        {
            throw new InvalidOperationException(message);
        }

        internal static void ThrowInvalidOperationException(string message, Exception inner)
        {
            throw new NotSupportedException(message, inner);
        }

        internal static void ThrowSetterTargetNull(ISettablePropertyDef context)
        {
            throw new InvalidOperationException(
                $"Attempting to set the '{context.Name}' property, but the target is null.'");
        }

        public static void ThrowGetterTargetNull(ISettablePropertyDef context)
        {
            throw new InvalidOperationException(
                $"Attempting to get the '{context.Name}' property, but the target is null.'");
        }

        public static void UnableToParseGuid(string s)
        {
            throw new InvalidOperationException(
                $"Attempting to parse '{s}',  as a Guid but it did not parse correctly.'");
        }

        public static void ThrowTypesDoNotMatch(Type actual, Type expectedType, ISettablePropertyDef context)
        {
            throw new InvalidOperationException(
                $"The actual type of the field '{context.Name}' was '{actual.FullName}',  but it was expected to be '{expectedType.FullName}'.'");
        }

        public static void PropertyTypeWasExpectedToBeAnEnumButWasNot(Type t)
        {
            throw new InvalidOperationException(
                $"Expecting Type '{t.FullName}' to be an enum, but it is not.'");
        }

        public static void StringIsNotValidEnumValue(string s, List<string> list, Type type)
        {
            throw new InvalidStringValueForEnumException(
                $"The value '{s}' is not a valid entry for enum type '{type.FullName}'. Valid values are {string.Join(", ", list)}.");

        }
    }
}
