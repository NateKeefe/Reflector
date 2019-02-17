// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorCodes.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The error codes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common
{
    /// <summary>The error codes.</summary>
    public enum ErrorCodes
    {
        /// <summary>The invalid argument.</summary>
        InvalidArgument = 1, 

        /// <summary>The connection error.</summary>
        ConnectionError, 

        /// <summary>The object not found.</summary>
        ObjectNotFound, 

        /// <summary>The unknown method.</summary>
        UnknownMethod, 

        /// <summary>The unknown operation.</summary>
        UnknownOperation, 

        /// <summary>The get object list error.</summary>
        GetObjectListError, 

        /// <summary>The no objects found.</summary>
        NoObjectsFound, 

        /// <summary>The no entities found.</summary>
        NoEntitiesFound, 

        /// <summary>The invalid date.</summary>
        InvalidDate, 

        /// <summary>The invalid format.</summary>
        InvalidFormat, 

        /// <summary>The limit exceeded.</summary>
        LimitExceeded, 

        /// <summary>The unknown error.</summary>
        UnknownError, 

        /// <summary>The unable to insert data.</summary>
        UnableToInsertData, 

        /// <summary>The multi objects not allowed.</summary>
        MultiObjectsNotAllowed, 

        /// <summary>The argument out of range.</summary>
        ArgumentOutOfRange, 

        /// <summary>The unable to update data.</summary>
        UnableToUpdateData, 

        /// <summary>The no records found.</summary>
        NoRecordsFound, 

        /// <summary>The unable to delete data.</summary>
        UnableToDeleteData, 

        /// <summary>The unable to upsert data.</summary>
        UnableToUpsertData, 

        /// <summary>The unable to bulk insert data.</summary>
        UnableToBulkInsertData, 

        /// <summary>The unable to bulk upsert data.</summary>
        UnableToBulkUpsertData, 

        /// <summary>The unable to bulk update data.</summary>
        UnableToBulkUpdateData, 

        /// <summary>The unable to bulk delete data.</summary>
        UnableToBulkDeleteData
    }
}