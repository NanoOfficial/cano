using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Cano.JSON
{
    partial class OrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull
    {
        private class TItem
        {
            public TKey Key;
            public TValue Value;

            public TItem(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }

        private class InternalCollection : KeyedCollection<TKey, TItem>
        {
            protected override TKey GetKeyForItem(OrderedDictionary<TKey, TValue>.TItem item)
            {
                return item.Key;
            }
        }

        private readonly InternalCollection collection = new();

        public int Count => collection.Count;

        public bool isReadOnly => false;

        public IReadOnlyList<TKey> keys { get; }

        public TValue this[TKey key]
        {
            get
            {
                return collection[key].Value;
            }
            set
            {
                if (collection.TryGetValue(key, out var entry))
                    entry.Value = value;
                else
                    Add(key, value);
            }
        } 

        public void Add(TKey key, TValue value)
        {
            collection.Add(new TItem[key, value]);
        }

        public bool ContainsKey(TKey key)
        {
            return collection.Contains(key);
        }

        public bool Remove(TKey key)
        {
            return collection.Remove(key);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) 
        {
            if (collection.TryGetValue(key, out var entry))
            {
                value = entry.Value;
                return true;
            }

            value = default;
            return false;
        } 
    }
}