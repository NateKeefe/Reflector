// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIndexed.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The Indexed interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Interfaces
{
    using Scribe.Connector.Common.Operation;

    /// <summary>The Indexed interface.</summary>
    /// <typeparam name="TValue">The type of the value of the indexed result. </typeparam>
    public interface IIndexed<out TValue> : IIndexed, IHasValue<TValue>
    {
    }

    /// <summary>The Indexed interface.</summary>
    public interface IIndexed
    {
        /// <summary>
        ///   Gets the index.
        /// </summary>
        int Index { get; }
    }
}