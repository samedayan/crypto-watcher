﻿using System;
using System.Collections.Generic;

namespace CryptoWatcher.Shared.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>
        (this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue defaultValue)
        {
            return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>
        (this IDictionary<TKey, TValue> dictionary,
            TKey key,
            Func<TValue> defaultValueProvider)
        {
            return dictionary.TryGetValue(key, out var value) ? value
                : defaultValueProvider();
        }
        public static string AsSplunkKeyValueString(this Dictionary<string, object> dictionary, string prefix = null)
        {
            var str = string.Empty;
            foreach (var item in dictionary)
            {
                if (item.Value is Dictionary<string, object> obj)
                {
                    str += obj.AsSplunkKeyValueString(item.Key) + ", ";
                }
                else
                {
                    // We skip those properties with spaces
                    if (!item.Value.ToString().Contains(" "))
                    {
                        var pref = prefix?.Length > 0 ? prefix + "_" : string.Empty;
                        // Cover scenarios like: Description=""
                        var value = (item.Value == null || item.Value.ToString() == string.Empty) ? "": item.Value;
                        str += pref + item.Key + "=" + item.Value + ", ";
                    }
                }
            }
            return str.Length > 0 ? str.Substring(0, str.Length - 2) : str;
        }
    }
}