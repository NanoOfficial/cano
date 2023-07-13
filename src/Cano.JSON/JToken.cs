using System.Text.Json;
using static Cano.JSON.Utility;

namespace Cano.JSON;

public abstract class JToken
{
    public const JToken? Null = null;

    public virtual JToken? this[int index]
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public virtual JToken? this[string key]
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public virtual bool AsBoolean()
    {
        return true;
    }

    public virtual T AsEnum<T>(T defaultValue = default, bool ignoreCase = false) where T : unmanaged, Enum
    {
        return defaultValue;
    }

    public virtual double AsNumber()
    {
        return double.NaN;
    }

    public virtual string AsString()
    {
        return ToString();
    }

    public virtual bool GetBoolean() => throw new InvalidCastException();

    public virtual T GetEnum<T>(bool ignoreCase = false) where T : unmanaged, Enum => throw new InvalidCastException();

    public int GetInt32()
    {
        double d = GetNumber();
        if (d % 1 != 0) throw new InvalidCastException();
        return checked((int)d);
    }

    public virtual double GetNumber() => throw new InvalidCastException();

    public virtual string GetString() => throw new InvalidCastException();

    public static JToken? Parse(ReadOnlySpan<byte> value, int max_nest = 100)
    {
        Utf8JsonReader reader = new(value, new JsonReaderOptions
        {
            AllowTrailingCommas = false,
            CommentHandling = JsonCommentHandling.Skip,
            MaxDepth = max_nest
        });
        try
        {
            JToken? json = Read(ref reader);
            if (reader.Read()) throw new FormatException();
            return json;
        }
        catch (JsonException ex)
        {
            throw new FormatException(ex.Message, ex);
        }
    }

    public static JToken? Parse(string value, int max_nest = 100)
    {
        return Parse(StrictUTF8.GetBytes(value), max_nest);
    }

    private static JToken? Read(ref Utf8JsonReader reader, bool skipReading = false)
    {
        if (!skipReading && !reader.Read()) throw new FormatException();
        return reader.TokenType switch
        {
            JsonTokenType.False => false,
            JsonTokenType.Null => Null,
            JsonTokenType.Number => reader.GetDouble(),
            JsonTokenType.StartArray => ReadArray(ref reader),
            JsonTokenType.StartObject => ReadObject(ref reader),
            JsonTokenType.String => ReadString(ref reader),
            JsonTokenType.True => true,
            _ => throw new FormatException(),
        };
    }

    private static JArray ReadArray(ref Utf8JsonReader reader)
    {
        JArray array = new();
        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.EndArray:
                    return array;
                default:
                    array.Add(Read(ref reader, skipReading: true));
                    break;
            }
        }
        throw new FormatException();
    }

    private static JObject ReadObject(ref Utf8JsonReader reader)
    {
        JObject obj = new();
        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.EndObject:
                    return obj;
                case JsonTokenType.PropertyName:
                    string name = ReadString(ref reader);
                    if (obj.Properties.ContainsKey(name)) throw new FormatException();
                    JToken? value = Read(ref reader);
                    obj.Properties.Add(name, value);
                    break;
                default:
                    throw new FormatException();
            }
        }
        throw new FormatException();
    }

    private static string ReadString(ref Utf8JsonReader reader)
    {
        try
        {
            return reader.GetString()!;
        }
        catch (InvalidOperationException ex)
        {
            throw new FormatException(ex.Message, ex);
        }
    }

    public byte[] ToByteArray(bool indented)
    {
        using MemoryStream ms = new();
        using Utf8JsonWriter writer = new(ms, new JsonWriterOptions
        {
            Indented = indented,
            SkipValidation = true
        });
        Write(writer);
        writer.Flush();
        return ms.ToArray();
    }

    public override string ToString()
    {
        return ToString(false);
    }

    public string ToString(bool indented)
    {
        return StrictUTF8.GetString(ToByteArray(indented));
    }

    internal abstract void Write(Utf8JsonWriter writer);

    public abstract JToken Clone();

    public JArray JsonPath(string expr)
    {
        JToken?[] objects = { this };
        if (expr.Length == 0) return objects;
        Queue<JPathToken> tokens = new(JPathToken.Parse(expr));
        JPathToken first = tokens.Dequeue();
        if (first.Type != JPathTokenType.Root) throw new FormatException();
        JPathToken.ProcessJsonPath(ref objects, tokens);
        return objects;
    }

    public static implicit operator JToken(Enum value)
    {
        return (JString)value;
    }

    public static implicit operator JToken(JToken?[] value)
    {
        return (JArray)value;
    }

    public static implicit operator JToken(bool value)
    {
        return (JBoolean)value;
    }

    public static implicit operator JToken(double value)
    {
        return (JNumber)value;
    }

    public static implicit operator JToken?(string? value)
    {
        return (JString?)value;
    }
}