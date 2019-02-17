// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardOperationResultsExtensions.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   StandardOperationResults extension methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Scribe.Connector.Common.Operation;
using Scribe.Core.ConnectorApi;

namespace Scribe.Connector.Common.Extensions
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mime;

    using Core.ConnectorApi.Actions;

    using Microsoft.SqlServer.Server;

    /// <summary>
    ///   StandardOperationResults extension methods.
    /// </summary>
    public static class StandardOperationResultsExtensions
    {
        /// <summary>StandardOperationResults as OperationResult.</summary>
        /// <param name="source">The source. </param>
        /// <returns>The Scribe.Core.ConnectorApi.Actions.OperationResult. </returns>
        public static OperationResult AsOperationResult(this StandardOperationResults source, StandardOperationInput input)
        {
            var result = new OperationResult(source.Results.Count);

            var dataEntityName = input.EntityName; 
            for (var i = 0; i < source.Results.Count; i++)
            {
                var sourceResult = source.Results[i];
                var inputItem = input.InputItems[i];
                if (!sourceResult.Success)
                {
                    result.ErrorInfo[i] = new ErrorResult
                        {
                            Description = sourceResult.ErrorDescription,
                            Detail = sourceResult.ErrorDetail,
                            Number = sourceResult.ErrorNumber 
                        };
                }

                var eq = inputItem.LookupCondition as ComparisonExpression;
                IDictionary<string, object> lookupPropertiesThatAreEqual = new Dictionary<string, object>();
                if (eq != null && eq.Operator == ComparisonOperator.Equal)
                {
                    lookupPropertiesThatAreEqual.Add(eq.LeftValue.Value.ToString(), eq.RightValue);
                }

                result.ObjectsAffected[i] = sourceResult.ObjectsAffected;
                result.Success[i] = sourceResult.Success;
                result.Output[i] = sourceResult.Output.MergeAsDataEntity(inputItem.Entity.Properties.Merge(lookupPropertiesThatAreEqual), dataEntityName);
            }

            return result;
        }
    }
}