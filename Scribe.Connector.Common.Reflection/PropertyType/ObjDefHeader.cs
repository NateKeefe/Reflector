namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using System.Collections.Generic;
    using System.Linq;

    using Scribe.Connector.Common.Reflection.Actions;
    using Scribe.Core.ConnectorApi.Metadata;

    public class ObjDefHeader : IObjDefHeader
    {
        public ObjDefHeader(string name, string description, bool hidden, IReadOnlyDictionary<string, ActionDef> actions, IReadOnlyDictionary<string, IPropDef> properties)
        {
            this.Name = name;
            this.Description = description;
            this.Hidden = hidden;
            this.Actions = actions;
            this.Properties = properties;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Hidden { get; set; }

        public IReadOnlyDictionary<string, ActionDef> Actions { get; }

        public IReadOnlyDictionary<string, IPropDef> Properties { get; }

        public static IObjectDefinition ToObjectDefinition(IObjDefHeader header)
        {
            return new ObjectDefinition
                       {
                           Description = header.Description, FullName = header.Name, Hidden = header.Hidden, Name = header.Name,
                           PropertyDefinitions = header.Properties.Values.Select(x => x.ToPropertyDefinition()).ToList(),
                            SupportedActionFullNames = header.Actions.Values.Select(a => a.FullName).ToList(),
                           RelationshipDefinitions = new List<IRelationshipDefinition>()
                       };
        }
    }
}