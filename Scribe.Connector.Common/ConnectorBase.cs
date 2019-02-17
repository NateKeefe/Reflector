// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectorBase.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   Abstract implementation of a connector that passes most calls to an Dispatcher layer, that will then translate Scribe constructs into the constructs of the underlying data source, and compose the appropriate handlers from the underlying system.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Scribe.Connector.Common.Operation;

namespace Scribe.Connector.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Scribe.Connector.Common.Interfaces;
    using Scribe.Connector.Common.Extensions;
    using Scribe.Connector.Common.Queries;
    using Scribe.Core.ConnectorApi;
    using Scribe.Core.ConnectorApi.Actions;
    using Scribe.Core.ConnectorApi.Exceptions;
    using Scribe.Core.ConnectorApi.Logger;
    using Scribe.Core.ConnectorApi.Query;

    /// <summary>Abstract implementation of a connector that passes most calls to an Dispatcher layer, that will then translate Scribe constructs into the constructs of the underlying data source, and compose the appropriate handlers from the underlying system.</summary>
    /// <typeparam name="TConnectionInfo">The Connection information that is passed into the Connect method of the underlying data source. </typeparam>
    public abstract class ConnectorBase<TConnectionInfo> : IConnector
    {
        /// <summary>
        ///   Gets ConnectorTypeId.
        /// </summary>
        public abstract Guid ConnectorTypeId { get; }

        /// <summary>
        ///   Gets a value indicating whether the connector is connected to its third-party.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return this.Connection.IsConnected;
            }
        }

        /// <summary>
        ///   Gets the connection.
        /// </summary>
        protected abstract IConnection<TConnectionInfo> Connection { get; }

        protected abstract string ConnectorName { get; }

        /// <summary>
        ///   Gets the Dispatcher.
        /// </summary>
        protected abstract IDispatcher<TConnectionInfo> Dispatcher { get; }

        /// <summary>
        ///   Gets a value indicating whether to treat all calls to PreConnect as requests for the connection UI.
        /// </summary>
        protected abstract bool TreatAllPreConnectsAsRequestForConnectionForm { get; }

        /// <summary>This method will attempt to connect to the third-party.</summary>
        /// <param name="properties">The collection of information required by this connector to connect to the third-party. </param>
        public void Connect(IDictionary<string, string> properties)
        {
            //using (new LogMethodExecution("Marketo", "Connect"))
            //{
                try
                {
                    var connectionDefinition = this.Connection.Create(properties);
                    this.Connection.Connect(connectionDefinition);
                }
                catch (ArgumentException argumentException)
                {
                    throw new InvalidConnectionException(argumentException);
                }
            //}
        }

        /// <summary>
        ///   This method will attempt to disconnect from the third-party.
        /// </summary>
        public void Disconnect()
        {
            using (new LogMethodExecution(ConnectorName, "Disconnect"))
            {
                this.Connection.Disconnect();
            }
        }

        /// <summary>This method will execute a Method returning a result.</summary>
        /// <param name="input">The input. </param>
        /// <returns>The Scribe.Core.ConnectorApi.Actions.MethodResult. </returns>
        public MethodResult ExecuteMethod(MethodInput input)
        {
            using (new LogMethodExecution(ConnectorName, "ExecuteMethod"))
            {
                this.ValidateMethod(input);
                var handler = this.Dispatcher.GetMethodHandler(input);
                return handler.Execute(input);
            }
        }

        private void ValidateMethod(MethodInput input)
        {   
        }

        /// <summary>This method will execute an operation returning a result. This method is also used in bulk operations.</summary>
        /// <param name="input">The input. </param>
        /// <returns>The Scribe.Core.ConnectorApi.Actions.OperationResult. </returns>
        public OperationResult ExecuteOperation(OperationInput input)
        {
            using (new LogMethodExecution(ConnectorName, "ExecuteOperation"))
            {
                this.ValidateOperation(input);
                var standardInput = new StandardOperationInput(input);

                var handler = this.Dispatcher.GetOperationHandler(
                    standardInput.OperationName, standardInput.EntityName, standardInput.AllowMultipleObject);

                var results = handler.Execute(standardInput.InputItems);

                var standardResults = new StandardOperationResults(results);
                return standardResults.AsOperationResult(standardInput);
            }
        }

        /// <summary>The connector will perform the query and pass the results back in an enumerable set of DataEntities. Each of which could be a set of objects</summary>
        /// <param name="query">The query. </param>
        /// <returns>The System.Collections.Generic.IEnumerable`1[T -&gt; Scribe.Core.ConnectorApi.DataEntity]. </returns>
        public IEnumerable<DataEntity> ExecuteQuery(Query query)
        {
            using (new LogMethodExecution(ConnectorName, "ExecuteQuery"))
            {
                var handler = this.Dispatcher.GetQueryHandler(query);
                return handler.Execute(query).Select( de => de.Conform(query));
            }
        }

        /// <summary>
        ///   Gets the metadata provider.
        /// </summary>
        /// <returns> The object that inherits the IMetadataProvider interface </returns>
        public IMetadataProvider GetMetadataProvider()
        {
            return this.Dispatcher.GetMetadataProvider();
        }

        /// <summary>This method will attempt to connect to the third-party and retrieve properties such as organizations databases etc. If the generic ConnectionUI is to be used, this method must return the serialized FormDefinition that will define connection properties.</summary>
        /// <param name="properties">The collection of information required by this connector to connect to the third-party. </param>
        /// <returns>The System.String. </returns>
        public string PreConnect(IDictionary<string, string> properties)
        {
            using (new LogMethodExecution(ConnectorName, "PreConnect"))
            {
                if (this.TreatAllPreConnectsAsRequestForConnectionForm)
                {
                    var formProvider = this.Dispatcher as IProvidesFormDefintion;

                    if (formProvider != null)
                    {
                        var formDefinition = formProvider.GetFormDefinition();
                        return formDefinition.Serialize();
                    }
                }

                // Implement appropriate dispatchers when needed.
                return string.Empty;
            }
        }

        /// <summary>The validate operation.</summary>
        /// <param name="input">The input.</param>
        /// <exception cref="FatalErrorException"></exception>
        private void ValidateOperation(OperationInput input)
        {
            if (input.Name == "Delete")
            {
                if (input.LookupCondition == null || input.LookupCondition.Length == 0)
                {
                    throw new FatalErrorException("No Lookup");
                }
            }
        }
    }
}