// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShouldRetry.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The should retry.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Behavior
{
    using System;

    /// <summary>The should retry.</summary>
    /// <param name="retryCount">The retry count.</param>
    /// <param name="lastException">The last exception.</param>
    /// <param name="delay">The delay.</param>
    public delegate bool ShouldRetry(int retryCount, Exception lastException, out TimeSpan delay);
}