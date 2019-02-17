namespace Scribe.Connector.Common.Queries
{
    using System.Collections.Generic;
    using System.Linq;

    using Scribe.Core.ConnectorApi;
    using Scribe.Core.ConnectorApi.Query;

    public static class QueryConformance
    {
        public static DataEntity Conform(DataEntity entity, Query query)
        {
            // Empty sets are better than null sets.
            if (entity.Children == null)
            {
                entity.Children = new EntityChildren();
            }

            // Poking at the data rather than creating a new one (a little iffy in this type of method, but there are two many gotchas in 
            // creating brand new DataEntities.
            if (query.RootEntity.ChildList != null)
            {
                var namedChildren = query.RootEntity.ChildList.Select(qe => qe.Name);

                foreach (var name in namedChildren.Where(name => !entity.Children.ContainsKey(name)))
                {
                    entity.Children.Add(name, new List<DataEntity>());
                }
            }

            // Strip out properties that were not requested
            entity.Properties.Strip(query.RootEntity.PropertyList);

            if (query.RootEntity.ChildList != null)
            {
                foreach (var childList in query.RootEntity.ChildList)
                {
                    var propertyList = childList.PropertyList;
                    var name = childList.Name;

                    if (!entity.Children.ContainsKey(name))
                    {
                        continue;
                    }

                    var entities = entity.Children[name];
                    if (entities == null)
                    {
                        continue;
                    }

                    foreach (var e in entities)
                    {
                        e.Properties.Strip(propertyList);
                    }
                }
            }

            return entity;
        }

        public static void Strip(this EntityProperties properties, List<string> propertyList)
        {
            var removable = properties.Keys.Except(propertyList).ToList();
            foreach (var unrequested in removable)
            {
                properties.Remove(unrequested);
            }
        }
    }
}