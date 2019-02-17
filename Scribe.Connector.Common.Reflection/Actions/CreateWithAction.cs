// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="CreateWithAction.cs">
//   
// </copyright>
// <summary>
//   The create with action.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Reflection.Actions
{
    using Scribe.Core.ConnectorApi.Metadata;

    /// <summary>The create with action.</summary>
    public class CreateWithAction : ActionDef
    {
        /// <summary>Initializes a new instance of the <see cref="CreateWithAction"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="supportsBulk">The supports bulk.</param>
        public CreateWithAction(string name, string description, bool supportsBulk)
            : base(name, description, KnownActions.CreateWith, supportsBulk, false)
        {
        }
    }
}