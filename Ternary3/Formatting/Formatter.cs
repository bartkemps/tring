using System.Runtime.CompilerServices;

namespace Ternary3.Formatting;

internal static class Formatter
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format(ITritArray value, ITernaryFormat format) 
        => new TernaryFormatter(format).Format(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format(Int3T value, ITernaryFormat format) 
        => new TernaryFormatter(format).Format((TritArray3)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format(Int9T value, ITernaryFormat format) 
        => new TernaryFormatter(format).Format((TritArray9)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format(Int27T value, ITernaryFormat format) 
        => new TernaryFormatter(format).Format((TritArray27)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format(TritArray3 trits, string? format, IFormatProvider? provider)
    {
        return GetFormatter(provider).Format(format, trits, provider);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format(TritArray9 trits, string? format, IFormatProvider? provider)
    {
        return GetFormatter(provider).Format(format, trits, provider);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format(TritArray27 trits, string? format, IFormatProvider? provider)
    {
        return GetFormatter(provider).Format(format, trits, provider);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format(Int3T trits, string? format, IFormatProvider? provider)
    {
        return GetFormatter(provider).Format(format, trits, provider);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format(Int9T trits, string? format, IFormatProvider? provider)
    {
        return GetFormatter(provider).Format(format, trits, provider);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Format(Int27T trits, string? format, IFormatProvider? provider)
    {
        return GetFormatter(provider).Format(format, trits, provider);
    }
    
    private static ICustomFormatter GetFormatter(IFormatProvider? provider)
    {
        var ternaryProvider = provider as TernaryFormatProvider ?? new TernaryFormatProvider(inner: provider);
        var formatter = ternaryProvider.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter;
        return formatter ?? new TernaryFormatter();
    }
    public static bool TryFormat(sbyte value, Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var str = Format((Int3T)value, format.ToString(), provider);
        var written = System.Text.Encoding.UTF8.GetBytes(str, utf8Destination);
        bytesWritten = written;
        return written > 0;
    }

    public static bool TryFormat(short value, Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var str = Format((Int9T)value, format.ToString(), provider);
        var written = System.Text.Encoding.UTF8.GetBytes(str, utf8Destination);
        bytesWritten = written;
        return written > 0;
    }

    public static bool TryFormat(long value, Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var str = Format((Int27T)value, format.ToString(), provider);
        var written = System.Text.Encoding.UTF8.GetBytes(str, utf8Destination);
        bytesWritten = written;
        return written > 0;
    }
    
    public static bool TryFormat(sbyte value, Span<char> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var str = Format((Int3T)value, format.ToString(), provider);
        if (str.Length > utf8Destination.Length)
        {
            bytesWritten = 0;
            return false;
        }
        str.AsSpan().CopyTo(utf8Destination);
        bytesWritten = str.Length;
        return true;
    }

    public static bool TryFormat(short value, Span<char> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var str = Format((Int9T)value, format.ToString(), provider);
        if (str.Length > utf8Destination.Length)
        {
            bytesWritten = 0;
            return false;
        }
        str.AsSpan().CopyTo(utf8Destination);
        bytesWritten = str.Length;
        return true;
    }

    public static bool TryFormat(long value, Span<char> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var str = Format((Int27T)value, format.ToString(), provider);
        if (str.Length > utf8Destination.Length)
        {
            bytesWritten = 0;
            return false;
        }
        str.AsSpan().CopyTo(utf8Destination);
        bytesWritten = str.Length;
        return true;
    }
}