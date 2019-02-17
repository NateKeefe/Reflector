// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertProperty.cs" company="Scribe Software Corporation">
//     Copyright © 1996-2013 Scribe Software Corp. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Scribe.Connector.Common.Converters
{
    /// <summary>
    /// Retrieves and converts the object property to its specific data type
    /// </summary>
    public class ConvertProperty
    {
        #region member variables
        private readonly IDictionary<string, string> _properties;
        #endregion

        #region ctor
        public ConvertProperty(IDictionary<string, string> properties)
        {
            _properties = properties;
        }
        #endregion

        /// <summary>
        /// Gets a Guid value out of the dictionary with errror handling.
        /// Sets to guid empty if not found
        /// </summary>
        /// <param name="key">name of key</param>
        /// <returns>value or Guid.Empty if not found</returns>
        public Guid ToGuid(string key)
        {
            var value = Guid.Empty;

            if (_properties != null && _properties.ContainsKey(key))
            {
                value = Guid.Parse(_properties[key]);
            }

            return value;
        }

        /// <summary>
        /// Gets a string value out of the dictionary with errror handling.
        /// Sets to string empty if not found
        /// </summary>
        /// <param name="key">name of key</param>
        /// <returns>value or string.empty if not found</returns>
        public string ToString(string key)
        {
            var value = string.Empty;

            if (_properties != null && _properties.ContainsKey(key))
            {
                value = _properties[key];
            }

            return value;
        }

        /// <summary>
        /// Gets a Int32 value out of the dictionary with errror handling.
        /// Sets to Int32.MinValue if not found
        /// </summary>
        /// <param name="key">name of key</param>
        /// <returns>value or Int32.MinValue if not found</returns>
        public Int32 ToInt32(string key)
        {
            var value = Int32.MinValue;

            if (_properties != null && _properties.ContainsKey(key))
            {
                value = Int32.Parse(_properties[key]);
            }

            return value;
        }


        /// <summary>
        /// Gets a Int64 value out of the dictionary with errror handling.
        /// Sets to Int64.MinValue if not found
        /// </summary>
        /// <param name="key">name of key</param>
        /// <returns>value or Int64.MinValue if not found</returns>
        public Int64 ToInt64(string key)
        {
            var value = Int64.MinValue;

            if (_properties != null && _properties.ContainsKey(key))
            {
                value = Int64.Parse(_properties[key]);
            }

            return value;
        }

        /// <summary>
        /// Gets a DateTime value out of the dictionary with errror handling.
        /// Sets to DateTime.MinValue if not found
        /// </summary>
        /// <param name="key">name of key</param>
        /// <returns>value or Int64.MinValue if not found</returns>
        public DateTime ToDateTime(string key)
        {
            var value = DateTime.MinValue;

            if (_properties != null && _properties.ContainsKey(key))
            {
                value = DateTime.Parse(_properties[key]);
            }

            return value;
        }

        /// <summary>
        /// Gets a DateTime value out of the dictionary with errror handling.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public DateTime ToDateTime(string key, IFormatProvider formatProvider)
        {
            var value = DateTime.MinValue;

            if (_properties != null && _properties.ContainsKey(key))
            {
                value = DateTime.Parse(_properties[key], formatProvider);
            }
            return value;
        }
        /// <summary>
        /// Gets a DateTime value out of the dictionary with errror handling.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public DateTime ToDateTime(string key, CultureInfo cultureInfo)
        {
            var value = DateTime.MinValue;

            if (_properties != null && _properties.ContainsKey(key))
            {
                value = DateTime.Parse(_properties[key], cultureInfo);
            }
            return value;
        }

        /// <summary>
        /// Gets a boolean value out of the dictionary with errror handling.
        /// Sets to false if not found
        /// </summary>
        /// <param name="key">name of key</param>
        /// <returns>value or false if not found</returns>
        public bool ToBoolean(string key)
        {
            var value = false;

            if (_properties != null && _properties.ContainsKey(key))
            {
                value = bool.Parse(_properties[key]);
            }

            return value;
        }
    }
}
