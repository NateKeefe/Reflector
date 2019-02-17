// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardOperationResults.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The standard operation results.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Scribe.Connector.Common.Operation
{
    /// <summary>
    ///   The standard operation results.
    /// </summary>
    public class StandardOperationResults
    {
        /// <summary>Initializes a new instance of the <see cref="StandardOperationResults"/> class.</summary>
        /// <param name="results">The results.</param>
        public StandardOperationResults(IEnumerable<ResultItem> results)
        {
            this.Results = new List<StandardResult>();
            foreach (var result in results)
            {
                bool success;
                if ((result.HasError == false) || (result.HasError && result.IsSuccessWithError))
                {
                    success = true;
                }
                else
                {
                    success = false;
                }

                int objectsAffected = result.ObjectsEffected;
                string errorDescription;
                string errorDetails;
                int errorNumber;

                if (result.Error != null)
                {
                    errorDescription = result.Error.Message;
                    errorDetails = result.Error.ToString();
                    errorNumber = 0;
                }
                else
                {
                    errorDescription = string.Empty;
                    errorDetails = string.Empty;
                    errorNumber = 0;
                }

                var standardResult = new StandardResult(success, objectsAffected, errorDescription, errorDetails, errorNumber)
                {
                    Output = result.Value
                };

                this.Results.Add(standardResult);
            }
        }

        /// <summary>
        ///   Gets the results.
        /// </summary>
        public IList<StandardResult> Results { get; private set; }
    }
}