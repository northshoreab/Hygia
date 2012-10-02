using System;
using System.Collections.Generic;

namespace Hygia.API.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Adds a key/value pair to the specified dictionary if the value is not null or empty.
        /// </summary>
        /// <param name="dictionary">
        /// The dictionary. 
        /// </param>
        /// <param name="key">
        /// The key. 
        /// </param>
        /// <param name="value">
        /// The value. 
        /// </param>
        public static void AddItemIfNotEmpty(this IDictionary<string, string> dictionary, string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (!string.IsNullOrEmpty(value))
            {
                dictionary[key] = value;
            }
        }
    }
}