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
        return checked ((int)d);
    }

    public virtual double GetNumber() => throw new InvalidCastException();

    public virtual string GetString() => throw new InvalidCastException();

    public static JToken? Parse(ReadOnlySpan<byte> value, int max_nest =100)
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
        } catch (JsonException ex)
        {
            throw new FormatException(ex.Message,ex);
        }
    }

    public static JToken? Read(ref Utf8JsonReader reader) 
    {
        return reader.TokenType switch
        {
            JsonTokenType.False => false,
            _ => throw new FormatException(),
        };
    }

    public string toString(bool indented)
    {
        
    }
    public abstract JToken Clone();

}