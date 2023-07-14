using System.Globalization;
using System.Text.Json;

namespace Cano.JSON
{
    public class JNumber: JToken
    {
        public static readonly long MAX_SAFE_INTEGER = (long)Math.Pow(2, 53) - 1;

        public double Value { get; }

        public JNumber(double value = 0)
        {
            if (!double.IsFinite(value)) throw new FormatException();
            this.Value = value;
        }

        public override bool AsBoolean()
        {
            return Value != 0;
        }

        public override string AsString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override double GetNumber() => Value;

        public override string ToString()
        {
            return AsString();
        }

        public override JToken Clone()
        {
            return this;
        }

    }
}