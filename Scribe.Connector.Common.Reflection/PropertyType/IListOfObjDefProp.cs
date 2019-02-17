namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using System.Collections.Generic;

    using Scribe.Core.ConnectorApi;

    public interface IListOfObjDefProp<T>
    {
        List<DataEntity> ToDataEntities(IEnumerable<T> data);

        IEnumerable<T> ToNatives(List<DataEntity> data);
    }
}