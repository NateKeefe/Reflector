using Newtonsoft.Json;
using Scribe.Core.ConnectorApi.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace MetadataComparer
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    using Scribe.Core.ConnectorApi.Metadata;

    public class Metadata
    {
        public Metadata(string s)
        {
            var m = Deserialize(s);
            this.Objects = m.Objects;
            this.Actions = m.Actions;
        }

        public Metadata(List<IActionDefinition> actions, List<IObjectDefinition> objects)
        {
            this.Objects = objects;
            this.Actions = actions;
        }

        public List<IActionDefinition> Actions { get; }

        public List<IObjectDefinition> Objects { get; }

        public static string Serialize(Metadata m)
        {
            var serializable = SerializableMetadata.MakeSerializable(m);
            return JsonConvert.SerializeObject(serializable);
        }

        public static Metadata Deserialize(string s)
        {
            var serializable = JsonConvert.DeserializeObject<SerializableMetadata>(s);
            var metadata = new Metadata(serializable.Actions.Cast<IActionDefinition>().ToList(), serializable.Objects.Cast<IObjectDefinition>().ToList());
            return metadata;
        }

        public class SerializableMetadata
        {
            public List<ActionDefinition> Actions { get; set; }
            public List<SerializableObjectDefintion> Objects { get; set; }

            public static SerializableMetadata MakeSerializable(Metadata m)
            {
                var sm = new SerializableMetadata();
                sm.Actions = m.Actions.Select(ConvertToConcrete).ToList();
                sm.Objects = m.Objects.Select(ConvertToConcrete).ToList();
                return sm;
            }

            private static ActionDefinition ConvertToConcrete(IActionDefinition ad)
            {
                return new ActionDefinition
                {
                    Description = ad.Description,
                    FullName = ad.FullName,
                    KnownActionType = ad.KnownActionType,
                    Name = ad.Name,
                    SupportsBulk = ad.SupportsBulk,
                    SupportsConstraints = ad.SupportsConstraints,
                    SupportsInput = ad.SupportsInput,
                    SupportsLookupConditions = ad.SupportsLookupConditions,
                    SupportsMultipleRecordOperations = ad.SupportsMultipleRecordOperations,
                    SupportsRelations = ad.SupportsRelations,
                    SupportsSequences = ad.SupportsSequences
                };
            }

            private static SerializableObjectDefintion ConvertToConcrete(IObjectDefinition od)
            {
                return new SerializableObjectDefintion
                {
                    Description = od.Description,
                    FullName = od.FullName,
                    Name = od.Name,
                    Hidden = od.Hidden,
                    SerializablePropertyDefinitions = od.PropertyDefinitions.Select(ConvertToConcrete).ToList(),
                    SerializableRelationshipDefinitions = od.RelationshipDefinitions.Select(ConvertToConcrete).ToList(),
                    SupportedActionFullNames = od.SupportedActionFullNames
                };
            }

            private static PropertyDefinition ConvertToConcrete(IPropertyDefinition od)
            {
                return new PropertyDefinition
                {
                    Description = od.Description,
                    FullName = od.FullName,
                    Name = od.Name,
                };
            }

            private static RelationshipDefinition ConvertToConcrete(IRelationshipDefinition rd)
            {
                return new RelationshipDefinition
                {
                    Description = rd.Description,
                    FullName = rd.FullName,
                    Name = rd.Name,
                    ThisObjectDefinitionFullName = rd.ThisObjectDefinitionFullName,
                    RelatedObjectDefinitionFullName = rd.RelatedObjectDefinitionFullName,
                    RelatedProperties = rd.RelatedProperties,
                    RelationshipType = rd.RelationshipType,
                    ThisProperties = rd.ThisProperties
                };
            }
        }   
    }

    public class SerializableObjectDefintion : IObjectDefinition
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Hidden { get; set; }
        public List<string> SupportedActionFullNames { get; set; }
        [JsonIgnore]
        public List<IPropertyDefinition> PropertyDefinitions { get { return this.SerializablePropertyDefinitions.Cast<IPropertyDefinition>().ToList(); }
            set { }
        }
        [JsonIgnore]
        public List<IRelationshipDefinition> RelationshipDefinitions
        {
            get { return this.SerializableRelationshipDefinitions.Cast<IRelationshipDefinition>().ToList(); }
            set { }
        }
        public List<PropertyDefinition> SerializablePropertyDefinitions { get; set; }
        public List<RelationshipDefinition> SerializableRelationshipDefinitions { get; set; }

    }
}