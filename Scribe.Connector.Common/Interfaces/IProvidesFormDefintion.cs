// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProvidesFormDefintion.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The ProvidesFormDefintion interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Interfaces
{
    using Scribe.Core.ConnectorApi.ConnectionUI;

    /// <summary>The ProvidesFormDefintion interface.</summary>
    public interface IProvidesFormDefintion
    {
        /// <summary>The get form definition.</summary>
        /// <returns>The <see cref="FormDefinition"/>.</returns>
        FormDefinition GetFormDefinition();
    }
}