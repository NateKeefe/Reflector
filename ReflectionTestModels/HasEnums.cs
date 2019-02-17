using Scribe.Connector.Common.Reflection;

namespace ReflectionTestModels
{
    [ObjectDefinition]
    public class HasEnums
    {
        [PropertyDefinition]
        public Options Address { get; set; }

        [PropertyDefinition(Description = "This is a precursor description.")]
        public Options WithDescription { get; set; }
    }
}