namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using Scribe.Core.ConnectorApi;
    using Scribe.Core.ConnectorApi.Query;

    public interface IObjDef<T> : IObjDefHeader
    {
        DataEntity Get(QueryEntity queryEntity);

        T Set(T target, DataEntity values);
    }
}