using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PhotosCategorier.DataStructure
{
    /// <summary>
    /// A MultiMap generic collection class that can store more than one value for a Key.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class Multimap<TKey, TValue> where TKey : IComparable
    {
        private readonly Dictionary<TKey, List<TValue>> dictMultiMap = new();

        public TKey[] Keys => dictMultiMap.Keys.ToArray();

        public List<TValue>[] Values => dictMultiMap.Values.ToArray();

        /// <summary>
        /// Construction of Multi map
        /// </summary>
        public Multimap()
        {
        }

        /// <summary>
        /// Construction copying from another Multi map
        /// </summary>
        /// <param name="other"></param>
        public Multimap(Multimap<TKey, TValue> other)
        {
            if (other == null) return;
            foreach (var pair in other.dictMultiMap)
            {
                var listValue = pair.Value.ToList();

                dictMultiMap.Add(pair.Key, listValue);
            }
        }

        public IEnumerator<KeyValuePair<TKey, List<TValue>>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, List<TValue>>>)dictMultiMap).GetEnumerator();
        }

        public bool TryGetValues(TKey key, [MaybeNullWhen(false)] out List<TValue> values)
        {
            if (dictMultiMap.TryGetValue(key, out var listValue))
            {
                values = listValue;
                return true;
            }
            values = null;
            return false;
        }

        public void Clear()
        {
            dictMultiMap.Clear();
        }

        /// <summary>
        /// Adds an element to the Multi map.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            if (dictMultiMap.TryGetValue(key, out var listToAdd))
            {
                listToAdd.Add(value);
            }
            else
            {
                listToAdd = new List<TValue>()
                {
                    value
                };
                dictMultiMap.Add(key, listToAdd);
            }
        }

        /// <summary>
        /// Removes the Key and all the values for an item.
        /// </summary>
        /// <param name="keyElement"></param>
        public bool RemoveAll(TKey keyElement)
        {
            var retVal = false;
            try
            {
                if (dictMultiMap.TryGetValue(keyElement, out var listToRemove))
                {
                    listToRemove.Clear();
                    dictMultiMap.Remove(keyElement);
                    retVal = true;
                }
            }
            catch
            {
                throw;
            }
            return retVal;
        }

        /// <summary>
        /// Deletes one Key and one Value from the Multi map.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public bool Remove(TKey key, TValue value)
        {
            if (!dictMultiMap.TryGetValue(key, out var listToRemove)) return false;
            var retVal = listToRemove.Remove(value);

            if (listToRemove.Count != 0) return retVal;
            dictMultiMap.Remove(key);

            return true;
        }
    }
}
