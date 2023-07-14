using System.Collections;
using System.Text.Json;

namespace Cano.JSON
{
    public class JArray : JContainer, IList<JToken?>
    {
        private readonly List<JToken?> items = new();

        public JArray(params JToken?[] items) : this((IEnumerable<JToken?>)items)
        {
        }

        public JArray(IEnumerable<JToken?> items)
        {
            this.items.AddRange(items);
        }

        public override JToken? this[int index]
        {
            get
            {
                return items[index];
            }
            set
            {
                items[index] = value;
            }
        }

        public override IReadOnlyList<JToken?> Children => items;

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(JToken? item)
        {
            items.Add(item);
        }

        public override string AsString()
        {
            return string.Join(",", items.Select(p => p?.AsString()));
        }

        public override void Clear()
        {
            items.Clear();
        }

        public bool Contains(JToken? item)
        {
            return items.Contains(item);
        }

        public IEnumerator<JToken?> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(JToken? item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, JToken? item)
        {
            items.Insert(index, item);
        }

        public bool Remove(JToken? item)
        {
            return items.Remove(item);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        internal override void Write(Utf8JsonWriter writer)
        {
            writer.WriteStartArray();
            foreach (JToken? item in items)
            {
                if (item is null)
                    writer.WriteNullValue();
                else
                    item.Write(writer);
            }
            writer.WriteEndArray();
        }

        public override JArray Clone()
        {
            var cloned = new JArray();

            foreach (JToken? item in items)
            {
                cloned.Add(item?.Clone());
            }

            return cloned;
        }

        public static implicit operator JArray(JToken?[] value)
        {
            return new JArray(value);
        }
    }
}