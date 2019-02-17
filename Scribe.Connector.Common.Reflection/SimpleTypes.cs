namespace Scribe.Connector.Common.Reflection
{
    using System;
    using System.Collections.Generic;

    internal static class SimpleTypes
    {
        public static HashSet<Type> Types = new HashSet<Type>
                                                {
                                                    typeof(string),
                                                    typeof(int),
                                                    typeof(long),
                                                    typeof(short),
                                                    typeof(uint),
                                                    typeof(ulong),
                                                    typeof(ushort),
                                                    typeof(decimal),
                                                    typeof(float),
                                                    typeof(double),
                                                    typeof(Guid),
                                                    typeof(DateTime),
                                                    typeof(bool),
                                                    typeof(char)
                                                };
    }
}