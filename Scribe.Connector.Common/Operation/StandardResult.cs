// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardResult.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   A more consumable result.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Scribe.Connector.Common.Operation
{
    /// <summary>
    ///   A more consumable result.
    /// </summary>
    public class StandardResult
    {
        /// <summary>Initializes a new instance of the <see cref="StandardResult"/> class.</summary>
        /// <param name="success">Whether or not the reuslt was a success. </param>
        /// <param name="objectsAffected">The number of objects affected. </param>
        /// <param name="errorDescription">The error description. </param>
        /// <param name="errorDetails">The error details. </param>
        /// <param name="errorNumber">The error number. </param>
        public StandardResult(
            bool success, int objectsAffected, string errorDescription, string errorDetails, int errorNumber)
        {
            this.Success = success;
            this.ObjectsAffected = objectsAffected;
            this.ErrorDescription = errorDescription;
            this.ErrorDetail = errorDetails;
            this.ErrorNumber = errorNumber;
        }

        /// <summary>
        ///   Gets the error description.
        /// </summary>
        public string ErrorDescription { get; private set; }

        /// <summary>
        ///   Gets the error detail.
        /// </summary>
        public string ErrorDetail { get; private set; }

        /// <summary>
        ///   Gets the error number.
        /// </summary>
        public int ErrorNumber { get; private set; }

        /// <summary>
        ///   Gets the objects affected.
        /// </summary>
        public int ObjectsAffected { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether success.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Data that is returned such as an id from an insert, etc. 
        /// </summary>
        public IDictionary<string, object> Output { get; set; }
    }
}