using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Scribe.Connector.Common.Reflection.PropertyType {
    public static class EnumConverters
    {
        public static Enum ConvertToEnum(Type enumType, object obj)         {
            if (!enumType.IsEnum) { ThrowHelper.PropertyTypeWasExpectedToBeAnEnumButWasNot(enumType); }

            if (obj is string s) return ConvertToEnum(enumType, s);
            if (obj is int i) return ConvertToEnum(enumType, i);

            ThrowHelper.StringIsNotValidEnumValue(obj.ToString(), new List<string>(), enumType);

            return null;
        }

        private static Enum ConvertToEnum(Type enumType, string s)
        {

            var names = Enum.GetNames(enumType);
            var lowerNames = names.Select(x => x.ToLower());

            if (lowerNames.Contains(s.ToLowerInvariant())) { return (Enum) Enum.Parse(enumType, s, true); }

            if (int.TryParse(s, out var i)) { return ConvertToEnum(enumType, i); }

            ThrowHelper.StringIsNotValidEnumValue(s, names.ToList(), enumType);

            return null; // yuck! Make the compiler happy.

        }

        private static Enum ConvertToEnum(Type enumType, int i)        {

            return (Enum) Enum.ToObject(enumType, i);
        }
    }
}