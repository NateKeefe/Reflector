// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITransientErrorDetectionStrategy.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The TransientErrorDetectionStrategy interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Interfaces
{
    using System;

    /// <summary>The TransientErrorDetectionStrategy interface.</summary>
    public interface ITransientErrorDetectionStrategy
    {
        /// <summary>The is transient.</summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool IsTransient(Exception exception);
    }
}