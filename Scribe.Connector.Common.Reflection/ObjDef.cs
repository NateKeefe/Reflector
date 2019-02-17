namespace Scribe.Connector.Common.Reflection
{
    using System.Collections.Generic;
    using System.Linq;

    using Scribe.Connector.Common.Reflection.PropertyType;
    using Scribe.Core.ConnectorApi.Metadata;

    public class ObjDef : IObjectDefinition
    {
       public ObjDef(
            string name,
            string desc,
            bool hidden,
            IEnumerable<IActionDefinition> actions,
            IEnumerable<IFullProp> props)
        {
            this.FullName = name;
            this.Name = name;
            this.Description = desc;
            this.Hidden = hidden;
            this.Actions = actions?.ToDictionary(x => x.FullName, x => x) ?? new Dictionary<string, IActionDefinition>();
            this.Properties = props?.ToDictionary(x => x.Name, x => x) ?? new Dictionary<string, IFullProp>();
            this.SupportedActionFullNames = this.Actions.Keys.ToList();
            this.PropertyDefinitions = this.Properties.Values.Select(x => x.ToPropertyDefinition()).ToList();
        }

        public string FullName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Hidden { get; set; }
        public IReadOnlyDictionary<string, IActionDefinition> Actions { get; }
        public IReadOnlyDictionary<string, IFullProp> Properties { get; }
        public List<string> SupportedActionFullNames { get; set; }

        public List<IPropertyDefinition> PropertyDefinitions { get; set; }

        public List<IRelationshipDefinition> RelationshipDefinitions { get; set; } =
            new List<IRelationshipDefinition>();
    }
}