using System.Text.Json;

namespace Cano.JSON
{
    public class JBoolean: JToken
    {
        public bool Value { get; }

        public JBoolean(bool value = false)
        {
            this.Value = value;
        }

        public override bool AsBoolean()
        {
            return Value;
        }

        public override bool GetBoolean() => Value;

        internal override void Write(Utf8JsonWriter writer)
        {
            writer.WriteBooleanValue(Value);
        }

        public override JBoolean Clone()
        {
            return this;
        }

        public static implicit operator JBoolean(bool value)
        {
            return new JBoolean(value);
        }
    }
}