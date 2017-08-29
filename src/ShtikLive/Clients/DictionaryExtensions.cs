using System.Collections.Generic;

namespace ShtikLive.Clients
{
    public static class DictionaryExtensions
    {
        public static string GetStringOrDefault(this IReadOnlyDictionary<string, object> dictionary, string key, string defaultValue)
        {
            if (!dictionary.TryGetValue(key, out var value)) return defaultValue;

            if (value is string s) return s;
            return value?.ToString() ?? defaultValue;
        }

        public static string GetStringOrDefault(
            this (IReadOnlyDictionary<string, object> first, IReadOnlyDictionary<string, object> second) pair, string key,
            string defaultValue)
        {
            if (!pair.first.TryGetValue(key, out var value))
            {
                if (!pair.second.TryGetValue(key, out value))
                {
                    return defaultValue;
                }
            }
            if (value is string s) return s;
            return value?.ToString() ?? defaultValue;
        }
    }
}