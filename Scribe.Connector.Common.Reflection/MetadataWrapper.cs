namespace Scribe.Connector.Common.Reflection
{
    using System;
    using System.Collections.Generic;

    using Scribe.Core.ConnectorApi;
    using Scribe.Core.ConnectorApi.Metadata;

    public class MetadataWrapper : IMetadataProvider
    {
        private readonly IMetadata metadata;

        /// <exception cref="ArgumentNullException">Condition.</exception>
        public MetadataWrapper(IMetadata metadata)
        {
            this.metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        }

        public void Dispose()
        {
        }

        public IEnumerable<IActionDefinition> RetrieveActionDefinitions()
        {
            return this.metadata.Actions.Values;
        }

        public IEnumerable<IObjectDefinition> RetrieveObjectDefinitions(bool shouldGetProperties = false, bool shouldGetRelations = false)
        {
            return this.metadata.Types.Values;
        }

        public IObjectDefinition RetrieveObjectDefinition(
            string objectName,
            bool shouldGetProperties = false,
            bool shouldGetRelations = false)
        {
            if (this.metadata.Types.TryGetValue(objectName, out var found))
            {
                return found;
            }

            return null;
        }

        /// <exception cref="NotSupportedException">Condition.</exception>
        public IEnumerable<IMethodDefinition> RetrieveMethodDefinitions(bool shouldGetParameters = false)
        {
            throw new NotSupportedException();
        }

        /// <exception cref="NotSupportedException">Condition.</exception>
        public IMethodDefinition RetrieveMethodDefinition(string objectName, bool shouldGetParameters = false)
        {
            throw new NotSupportedException();
        }

        public void ResetMetadata()
        {
        }
    }
}