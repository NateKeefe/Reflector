using Scribe.Connector.Common.Reflection;

namespace ReflectionTestModels {
    [ObjectDefinition]
    public class HasWeirdEnums
    {
        [PropertyDefinition]
        public Unit NoEnum { get; set; }

        [PropertyDefinition]
        public Offset OffsetEnum { get; set; }

        [PropertyDefinition]
        public ByteEnum ByteEnum { get; set; }
    }
}