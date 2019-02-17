// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputItem.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   An item that represents both the data entity and its Lookup condition.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Interfaces
{
    using Scribe.Core.ConnectorApi;

    /// <summary>
    ///   An item that represents both the data entity and its Lookup condition.
    /// </summary>
    public class InputItem
    {
        /// <summary>Initializes a new instance of the <see cref="InputItem"/> class.</summary>
        /// <param name="dataEntity">The data entity. </param>
        /// <param name="lookupCondition">The lookup condition. </param>
        public InputItem(DataEntity dataEntity, Expression lookupCondition)
        {
            this.Entity = dataEntity;
            this.LookupCondition = lookupCondition;
        }

        /// <summary>
        ///   Gets the entity.
        /// </summary>
        public DataEntity Entity { get; private set; }

        /// <summary>
        ///   Gets the lookup condition.
        /// </summary>
        public Expression LookupCondition { get; private set; }
    }
}