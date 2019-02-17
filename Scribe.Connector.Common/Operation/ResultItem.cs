// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultItem.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   TODO: Update summary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Scribe.Connector.Common.Exceptions;
using Scribe.Core.ConnectorApi.Exceptions;

namespace Scribe.Connector.Common.Operation
{
    using Scribe.Connector.Common.Interfaces;

    /// <summary>
    ///   The result of an endpoint request such as an Insert, Update, etc.
    /// </summary>
    public class ResultItem : IResult, IHasValue<IDictionary<string, object>>
    {
        private readonly Exception error;

        private readonly bool hasError;

        private readonly int objectsEffected;

        private readonly IDictionary<string, object> value;

        /// <summary>Initializes a new instance of the <see cref="ResultItem"/> class.</summary>
        /// <param name="objectsEffected">The objects effected. </param>
        /// <param name="hasError">The has error. </param>
        /// <param name="error">The error. </param>
        public ResultItem(int objectsEffected, bool hasError, Exception error = null)
        {
            this.objectsEffected = objectsEffected;
            this.hasError = hasError;
            this.error = error;
            this.value = new Dictionary<string, object>();
        }

        public ResultItem(int objectsEffected, IDictionary<string, object> values)
        {
            this.objectsEffected = objectsEffected;
            this.hasError = false;
            this.error = null;
            this.value = new Dictionary<string, object>(values);
        }

        /// <summary>
        ///   Gets the error.
        /// </summary>
        public Exception Error
        {
            get
            {
                return this.error;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether has error.
        /// </summary>
        public bool HasError
        {
            get
            {
                return this.hasError;
            }
        }

        /// <summary>
        ///   If an error occured this will indicate whether or not it is a fatal error. 
        ///   Non-Fatal errors should be treated as row errors.
        /// </summary>
        public bool IsFatalError
        {
            get
            {
                return this.HasError && this.Error is FatalErrorException;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is success with error.
        /// </summary>
        public bool IsSuccessWithError
        {
            get
            {
                var isSuccessWithError = false;
                if (HasError)
                {
                    if (Error is RecordNotFoundException || Error is DuplicateKeyException)
                    {
                        isSuccessWithError = true;
                    }
                }

                return isSuccessWithError;
            }
        }

        /// <summary>
        ///   Gets the objects affected.
        /// </summary>
        public int ObjectsEffected
        {
            get
            {
                return this.objectsEffected;
            }
        }

        /// <summary>
        /// Data that is returned such as an id from an insert, etc. 
        /// </summary>
        public IDictionary<string, object> Value
        {
            get
            {
                return this.value;
            }
        }
    }
}