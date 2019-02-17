// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Indexed.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   An indexed item.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Interfaces
{
    /// <summary>An indexed item.</summary>
    /// <typeparam name="TValue">The type of the Value. </typeparam>
    public class Indexed<TValue> : IIndexed<TValue>
    {
        /// <summary>Initializes a new instance of the <see cref="Indexed{TValue}"/> class.</summary>
        /// <param name="value">The value. </param>
        /// <param name="index">The index. </param>
        public Indexed(TValue value, int index)
        {
            this.Index = index;
            this.Value = value;
        }

        /// <summary>
        ///   Gets the index.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        ///   Gets the value.
        /// </summary>
        public TValue Value { get; private set; }
    }
}