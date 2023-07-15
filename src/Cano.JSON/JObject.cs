using System.Text.Json;

namespace Cano.JSON
{
    public class JObject : JContainer
    {
        private readonly OrderedDictionary<string, JToken?> properties = new();

        public IDictionary<string, JToken?> Properties => properties;

        public override JToken? this[string name]
        {
            get
            {
                if (Properties.TryGetValue(name, out JToken? value))
                    return value;
                return null;
            }
            set
            {
                Properties[name] = value;
            }
        }

        public override IReadOnlyList<JToken?> Children => properties.Values;

        public bool ContainsProperty(string key)
        {
            return Properties.ContainsKey(key);
        }

        public override void Clear() => properties.Clear();

        internal override void Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            foreach (var (key, value) in Properties)
            {
                writer.WritePropertyName(key);
                if (value is null)
                    writer.WriteNullValue();
                else
                    value.Write(writer);
            }
            writer.WriteEndObject();
        }

        public override JObject Clone()
        {
            var cloned = new JObject();

            foreach (var (key, value) in Properties)
            {
                cloned[key] = value != null ? value.Clone() : Null;
            }

            return cloned;
        }
    }
}