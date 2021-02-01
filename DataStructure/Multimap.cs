using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PhotosCategorier.DataStructure
{
    /// <summary>
    /// A MultiMap generic collection class that can store more than one value for a Key.
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="Value"></typeparam>
    public class Multimap<Key, Value> where Key : IComparable
    {
        private readonly Dictionary<Key, List<Value>> dictMultiMap;

        public Key[] Keys => dictMultiMap.Keys.ToArray();

        public List<Value>[] Values => dictMultiMap.Values.ToArray();

        /// <summary>
        /// Construction of Multi map
        /// </summary>
        public Multimap()
        {
            dictMultiMap = new Dictionary<Key, List<Value>>();
        }

        /// <summary>
        /// Construction copying from another Multi map
        /// </summary>
        /// <param name="other"></param>
        public Multimap(Multimap<Key, Value> other)
        {
            dictMultiMap = new Dictionary<Key, List<Value>>();

            if (other != null)
            {
                foreach (var pair in other.dictMultiMap)
                {
                    var listValue = new List<Value>();

                    foreach (var value in pair.Value)
                    {
                        listValue.Add(value);
                    }
                    dictMultiMap.Add(pair.Key, listValue);
                }
            }
        }

        public IEnumerator<KeyValuePair<Key, List<Value>>> GetEnumerator()
        {
            foreach (var item in dictMultiMap)
            {
                yield return item;
            }
        }

        public bool TryGetValues(Key key, [MaybeNullWhen(false)] out List<Value> values)
        {
            if (dictMultiMap.TryGetValue(key, out var listvValue))
            {
                values = listvValue;
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
        public void Add(Key key, Value value)
        {
            try
            {
                if (dictMultiMap.TryGetValue(key, out var listToAdd))
                {
                    listToAdd.Add(value);
                }
                else
                {
                    listToAdd = new List<Value>
                {
                    value
                };
                    dictMultiMap.Add(key, listToAdd);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Removes the Key and all the values for an item.
        /// </summary>
        /// <param name="KeyElement"></param>
        public bool RemoveAll(Key KeyElement)
        {
            var retVal = false;
            try
            {
                if (dictMultiMap.TryGetValue(KeyElement, out var listToRemove))
                {
                    listToRemove.Clear();
                    dictMultiMap.Remove(KeyElement);
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
        public bool Remove(Key key, Value value)
        {
            var retVal = false;
            try
            {
                if (dictMultiMap.TryGetValue(key, out var listToRemove))
                {
                    retVal = listToRemove.Remove(value);

                    if (listToRemove.Count == 0)
                    {
                        listToRemove = null;
                        dictMultiMap.Remove(key);
                        retVal = true;
                    }
                }
            }
            catch
            {
                throw;
            }

            return retVal;
        }
    }
}
