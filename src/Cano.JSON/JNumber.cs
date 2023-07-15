using System.Globalization;
using System.Text.Json;

namespace Cano.JSON
{
    public class JNumber : JToken
    {
        public static readonly long MAX_SAFE_INTEGER = (long)Math.Pow(2, 53) - 1;

        public static readonly long MIN_SAFE_INTEGER = -MAX_SAFE_INTEGER;

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

        public override double AsNumber()
        {
            return Value;
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

        public override T AsEnum<T>(T defaultValue = default, bool ignoreCase = false)
        {
            Type enumType = typeof(T);
            object value;
            try
            {
                value = Convert.ChangeType(Value, enumType.GetEnumUnderlyingType());
            }
            catch (OverflowException)
            {
                return defaultValue;
            }
            object result = Enum.ToObject(enumType, value);
            return Enum.IsDefined(enumType, result) ? (T)result : defaultValue;
        }

        public override T GetEnum<T>(bool ignoreCase = false)
        {
            Type enumType = typeof(T);
            object value;
            try
            {
                value = Convert.ChangeType(Value, enumType.GetEnumUnderlyingType());
            }
            catch (OverflowException)
            {
                throw new InvalidCastException();
            }
            object result = Enum.ToObject(enumType, value);
            if (!Enum.IsDefined(enumType, result))
                throw new InvalidCastException();
            return (T)result;
        }

        internal override void Write(Utf8JsonWriter writer)
        {
            writer.WriteNumberValue(Value);
        }

        public override JNumber Clone()
        {
            return this;
        }

        public static implicit operator JNumber(double value)
        {
            return new JNumber(value);
        }
    }
}