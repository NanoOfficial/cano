using System.Text.Json;

namespace Cano.JSON
{
    public class JObject: JContainer
    {
        private readonly OrderedDictionary<string, JToken?>  properties_ = new();

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

        public override IReadOnlyList<JToken?> Children => properties.Value;

        public bool ContainsProperty(string key)
        {
            return Properties.ContainsKey(key);
        }

        public override void Clear() => Properties.Clear();
    }
}