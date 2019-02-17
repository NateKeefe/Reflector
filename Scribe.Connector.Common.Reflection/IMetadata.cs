namespace Scribe.Connector.Common.Reflection
{
    using System.Collections.Generic;

    using Scribe.Core.ConnectorApi.Metadata;

    public interface IMetadata
    {
        IReadOnlyDictionary<string, IActionDefinition> Actions { get; }
        IReadOnlyDictionary<string, IObjectDefinition> Types { get; }
    }
}