// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExecuteOn.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   The ExecuteOn interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Interfaces
{
    using System;

    /// <summary>The ExecuteOn interface.</summary>
    /// <typeparam name="X"></typeparam>
    public interface IExecuteOn<X>
    {
        /// <summary>The execute on.</summary>
        /// <param name="func">The func. </param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <returns>The System.Func`2[T -&gt; T, TResult -&gt; U]. </returns>
        Func<T, U> ExecuteOn<T, U>(Func<T, X, U> func);
    }
}