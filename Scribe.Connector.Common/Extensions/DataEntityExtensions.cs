namespace Scribe.Connector.Common.Extensions
{
    using System.Collections.Generic;

    using Scribe.Core.ConnectorApi;
    using Scribe.Core.ConnectorApi.Query;

    public static class DataEntityExtensions
    {
        public static IDictionary<string, object> Merge(
            this IDictionary<string, object> source, IDictionary<string, object> toMerge)
        {
            var dictionary = source == null ? new Dictionary<string, object>() : new Dictionary<string, object>(source);

            if (toMerge == null)
            {
                return dictionary;
            }

            foreach (var keyVal in toMerge)
            {
                if (!dictionary.ContainsKey(keyVal.Key))
                {
                    dictionary.Add(keyVal.Key, keyVal.Value);
                }
            }

            return dictionary;
        }

        public static DataEntity MergeAsDataEntity(
            this IDictionary<string, object> source, IDictionary<string, object> toMerge, string name)
        {
            var dictionary = source.Merge(toMerge);
            var properties = new EntityProperties();
            foreach (var prop in dictionary)
            {
                properties.Add(prop.Key, prop.Value);
            }

            var de = new DataEntity(name) { Properties = properties };

            return de;
        }
 
    }
}