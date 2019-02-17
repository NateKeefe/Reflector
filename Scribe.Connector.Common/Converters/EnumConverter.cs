// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumConverter.cs" company="Scribe Software Corporation">
//   Copyright © 1996-2012 Scribe Software Corp. All rights reserved.
// </copyright>
// <summary>
//   Conversion for Enums.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Scribe.Connector.Common.Converters
{
    using System;

    /// <summary>
    ///   Conversion for Enums.
    /// </summary>
    public static class EnumConverter
    {
        /// <summary>Converts an object to an Enum of type T.</summary>
        /// <param name="enumObject">The object to convert. </param>
        /// <typeparam name="T">The type of the Enum. Note that T cannot be constrained to Enum, but in practice it should be. </typeparam>
        /// <returns>An Enum of type T. </returns>
        public static T Convert<T>(object enumObject) where T : struct
        {
            T temp;
            if (Enum.TryParse(enumObject.ToString(), true, out temp))
            {
                return temp;
            }

            throw new InvalidCastException(
                string.Format("Unable to convert {0} into an Enum of type {1}.", enumObject, typeof(T).FullName));
        }
    }
}