namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using System.Collections.Generic;

    using Scribe.Core.ConnectorApi;
    using Scribe.Core.ConnectorApi.Query;

    public interface IDataEntityProperty
    {
        string PropertyType { get; }

        DataEntity Get(object target);
        DataEntity Get(object target, QueryEntity queryEntity);

        void Set(object target, DataEntity val);
    }

    public interface IDataEntityListProperty
    {
        string PropertyType { get;  }
        List<DataEntity> Get(object target);
        List<DataEntity> Get(object target, QueryEntity queryEntity);

        void Set(object target, IEnumerable<DataEntity> val);
    }
}