using System.Collections;
using System.Text.Json;

namespace Cano.JSON
{
    public class JArray : JContainer, IList<JToken?>
    {

        public override JToken Clone()
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