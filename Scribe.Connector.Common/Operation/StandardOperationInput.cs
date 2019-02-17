// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardOperationInput.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   A more consumable OperationInput.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Scribe.Connector.Common.Interfaces;
using Scribe.Core.ConnectorApi;
using Scribe.Core.ConnectorApi.Actions;

namespace Scribe.Connector.Common.Operation
{
    /// <summary>
    ///   A more consumable OperationInput.
    /// </summary>
    public class StandardOperationInput
    {
        /// <summary>Initializes a new instance of the <see cref="StandardOperationInput"/> class.</summary>
        /// <param name="input">The OperationInput. </param>
        public StandardOperationInput(OperationInput input)
        {
            if (string.IsNullOrWhiteSpace(input.Name))
            {
                throw new Exception(Resources.Connector.OperationInputIsNull);
            }

            if (input.Input == null || input.Input.Length < 1)
            {
                throw new Exception(Resources.Connector.OperationInputwithnodataExc);
            }

            this.AllowMultipleObject = input.AllowMultipleObject;
            this.InputItems = new List<InputItem>();
            string entityName = null;

            for (var i = 0; i < input.Input.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(entityName))
                {
                    entityName = input.Input[i].ObjectDefinitionFullName;
                }

                Expression lookupCondition = null;
                if (input.LookupCondition != null)
                {
                    lookupCondition = input.LookupCondition[i];
                }

                this.InputItems.Add(new InputItem(input.Input[i], lookupCondition));
            }

            this.OperationName = input.Name;
            this.EntityName = entityName;
        }

        /// <summary>
        ///   Gets a value indicating whether allow multiple object.
        /// </summary>
        public bool AllowMultipleObject { get; private set; }

        /// <summary>
        ///   Gets the entity name.
        /// </summary>
        public string EntityName { get; private set; }

        /// <summary>
        ///   Gets the input.
        /// </summary>
        public IList<InputItem> InputItems { get; private set; }

        /// <summary>
        ///   Gets the operation name.
        /// </summary>
        public string OperationName { get; private set; }
    }
}