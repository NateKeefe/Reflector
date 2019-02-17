namespace Scribe.Connector.Common.Reflection
{
    using System;
    using System.Collections.Generic;


    using Scribe.Core.ConnectorApi.Metadata;

    public class Metadata : IMetadata
    {
        public Metadata(IReadOnlyDictionary<string, IActionDefinition> actions, IReadOnlyDictionary<string, IObjectDefinition> types)
        {
            this.Actions = actions ?? throw new ArgumentNullException(nameof(actions));
            this.Types = types ?? throw new ArgumentNullException(nameof(types));
        }
        public IReadOnlyDictionary<string, IActionDefinition> Actions { get; }

        public IReadOnlyDictionary<string, IObjectDefinition> Types { get; }
    }
}