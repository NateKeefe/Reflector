// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConnection.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The Connection interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Interfaces
{
    using System.Collections.Generic;

    /// <summary>The Connection interface.</summary>
    /// <typeparam name="T"></typeparam>
    public interface IConnection<T>
    {
        /// <summary>Gets the connection definition.</summary>
        T ConnectionDefinition { get; }

        /// <summary>Gets a value indicating whether is connected.</summary>
        bool IsConnected { get; }

        /// <summary>The connect.</summary>
        /// <param name="connectionDefinition">The connection definition.</param>
        void Connect(T connectionDefinition);

        /// <summary>The create.</summary>
        /// <param name="properties">The properties.</param>
        /// <returns>The <see cref="T"/>.</returns>
        T Create(IDictionary<string, string> properties);

        /// <summary>The disconnect.</summary>
        void Disconnect();
    }
}