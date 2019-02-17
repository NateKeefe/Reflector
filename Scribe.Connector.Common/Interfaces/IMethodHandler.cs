// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMethodHandler.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The MethodHandler interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Interfaces
{
    using Scribe.Core.ConnectorApi.Actions;

    /// <summary>The MethodHandler interface.</summary>
    public interface IMethodHandler
    {
        /// <summary>The execute.</summary>
        /// <param name="input">The input.</param>
        /// <returns>The <see cref="MethodResult"/>.</returns>
        MethodResult Execute(MethodInput input);
    }
}