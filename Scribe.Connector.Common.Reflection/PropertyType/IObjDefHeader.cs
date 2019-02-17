namespace Scribe.Connector.Common.Reflection.PropertyType
{
    using System.Collections.Generic;

    using Scribe.Connector.Common.Reflection.Actions;

    public interface IObjDefHeader
    {
        string Name { get; set; }

        string Description { get; set; }

        bool Hidden { get; set; }

        IReadOnlyDictionary<string, ActionDef> Actions { get; }

        IReadOnlyDictionary<string, IPropDef> Properties { get; }
    }
}