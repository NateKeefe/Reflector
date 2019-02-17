namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using System;

    using Scribe.Connector.Common.Reflection;

    public static class SimpleTypeConverters
    {
        public static Int32 ConvertToInt32(object obj)
        {
            return Convert.ToInt32(obj);
        }

        public static byte ConvertToByte(object o)
        {
            return Convert.ToByte(o);
        }

        public static Byte[] ConvertToByteArray(object o)
        {
            return (Byte[]) o;
        }

        public static bool ConvertToBoolean(object o)
        {
            return Convert.ToBoolean(o);
        }

        public static DateTime ConvertToDateTime(object o)
        {
            return Convert.ToDateTime(o);
        }

        public static short ConvertToInt16(object o)
        {
            return Convert.ToInt16(o);
        }

        public static long ConvertToInt64(object o)
        {
            return Convert.ToInt64(o);
        }

        public static decimal ConvertToDecimal(object o)
        {
            return Convert.ToDecimal(o);
        }

        public static double ConvertToDouble(object o)
        {
            return Convert.ToDouble(o);
        }

        public static float ConvertToSingle(object o)
        {
            return Convert.ToSingle(o);
        }

        public static Guid ConvertToGuid(object o)
        {
            if (o is Guid g) return g;
            if (Guid.TryParse(o.ToString(), out g))
            {
                return g;
            }

            ThrowHelper.UnableToParseGuid(o.ToString());
            // Make the compiler happy -- Adding a throw helper to reduce code at this point
            // Most branches should succeed, so no need to make *this* function take on cost of trycatch
            // mechanisms.
            return Guid.Empty;
        }

        public static string ConvertToString(object o)
        {
            return o == null ? null : Convert.ToString(o);
        }

        public static char ConvertToChar(object o)
        {
            return Convert.ToChar(o);
        }
    }
}