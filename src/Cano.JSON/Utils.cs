using System.Text;

namespace Cano.JSON;


/// @brief: Utility
static class Utility
{
    /// @brief: Encoding
    public static Encoding StrictUTF8 { get; }

    /// @brief: Utility
    static Utility()
    {
        StrictUTF8 = (Encoding)Encoding.UTF8.Clone();
        StrictUTF8.DecoderFallback = DecoderFallback.ExceptionFallback;
        StrictUTF8.EncoderFallback = EncoderFallback.ExceptionFallback;
    }
}