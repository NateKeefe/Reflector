// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGetValueAsKeyValuePair.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   TODO The GetValueAsKeyValuePair interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    ///   TODO The GetValueAsKeyValuePair interface.
    /// </summary>
    internal interface IGetValueAsKeyValuePair
    {
        /// <summary>TODO The get as key value pair.</summary>
        /// <param name="key">TODO The key. </param>
        /// <param name="val">TODO The val. </param>
        /// <returns>The System.Collections.Generic.IEnumerable`1[T -&gt; System.Collections.Generic.KeyValuePair`2[TKey -&gt; System.String, TValue -&gt; System.Object]]. </returns>
        IEnumerable<KeyValuePair<string, object>> GetAsKeyValuePair(string key, object val);
    }
}