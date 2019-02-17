namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using Scribe.Core.ConnectorApi;

    public interface IObjDefProp<T>
    {
        DataEntity ToDataEntity(T data);

        T ToNative(DataEntity data);
    }
}