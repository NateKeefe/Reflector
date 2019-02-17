// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Operation.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The operation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Operation
{
    /// <summary>
    ///   The type of operation.
    /// </summary>
    public enum OperationType
    {
        Unknown,
        Create, 
        Upsert, 
        Update, 
        Delete, 
        Query
    }
}