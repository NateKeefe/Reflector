namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Scribe.Core.ConnectorApi;

    public static class PropertyTypeExt
    {
        private static IReadOnlyDictionary<Type, string> simpleTypes = new ReadOnlyDictionary<Type, string>(
            new Dictionary<Type, string>
                {
                    { typeof(string), typeof(string).FullName },
                    { typeof(int), typeof(int).FullName },
                    { typeof(short), typeof(short).FullName },
                    { typeof(long), typeof(long).FullName },
                    { typeof(DateTime), typeof(DateTime).FullName },
                    { typeof(bool), typeof(bool).FullName },
                    { typeof(Guid), typeof(Guid).FullName },
                    { typeof(float), typeof(float).FullName },
                    { typeof(double), typeof(double).FullName },
                    { typeof(decimal), typeof(decimal).FullName }
                });

        private static IReadOnlyDictionary<Type, string> builtInTypes = new ReadOnlyDictionary<Type, string>(
        new Dictionary<Type, string>
        {
            { typeof(string), typeof(string).FullName },
            { typeof(int), typeof(int).FullName },
            { typeof(short), typeof(short).FullName },
            { typeof(long), typeof(long).FullName },
            { typeof(DateTime), typeof(DateTime).FullName },
            { typeof(bool), typeof(bool).FullName },
            { typeof(Guid), typeof(Guid).FullName },
            { typeof(float), typeof(float).FullName },
            { typeof(double), typeof(double).FullName },
            { typeof(decimal), typeof(decimal).FullName },
            { typeof(int?), typeof(int).FullName },
            { typeof(short?), typeof(short).FullName },
            { typeof(long?), typeof(long).FullName },
            { typeof(DateTime?), typeof(DateTime).FullName },
            { typeof(bool?), typeof(bool).FullName },
            { typeof(Guid?), typeof(Guid).FullName },
            { typeof(float?), typeof(float).FullName },
            { typeof(double?), typeof(double).FullName },
            { typeof(decimal?), typeof(decimal).FullName },
        });


        // The result of this exercise is that for lists
        // Gets should always return a List<T>
        // Sets must be converted from List<T> to the specific Target (which can be done in the MakeSetter code
        // So we can have the 
        private static Dictionary<Type, Type> mappings = new Dictionary<Type, Type>
        {
            { typeof(string), typeof(string) },
            { typeof(int), typeof(int) },
            { typeof(int?), typeof(int?) },

            { typeof(List<int>), typeof(List<int>) },
            { typeof(List<int?>), typeof(List<int?>) },
            { typeof(IEnumerable<int>), typeof(List<int>) },
            { typeof(IEnumerable<int?>), typeof(List<int?>) },
            { typeof(int[]), typeof(List<int>) },
            { typeof(int?[]), typeof(List<int>) },

            { typeof(DataEntity), typeof(DataEntity) },
            { typeof(IEnumerable<DataEntity>), typeof(List<DataEntity>) },
            { typeof(DataEntity[]), typeof(List<DataEntity>) }
        };
    }
}