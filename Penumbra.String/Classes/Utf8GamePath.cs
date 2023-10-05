using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Penumbra.String.Classes;

/// <summary>
/// A GamePath that verifies some invariants based on a ByteString.
/// The Invariants are being smaller than <see cref="MaxGamePathLength"/>,
/// and containing forward-slashes as separators.
/// </summary>
[JsonConverter(typeof(Utf8GamePathConverter))]
public readonly struct Utf8GamePath : IEquatable<Utf8GamePath>, IComparable<Utf8GamePath>, IDisposable
{
    /// <summary>
    /// The maximum length Penumbra accepts.
    /// </summary>
    public const int MaxGamePathLength = 2 << 10;

    /// <summary>
    /// Return the original path.
    /// </summary>
    public readonly ByteString Path;

    /// <summary> An empty path. </summary>
    public static readonly Utf8GamePath Empty = new(ByteString.Empty);

    internal Utf8GamePath(ByteString s)
        => Path = s;

    /// <inheritdoc cref="ByteString.Length"/>
    public int Length
        => Path.Length;

    /// <inheritdoc cref="ByteString.IsEmpty"/>
    public bool IsEmpty
        => Path.IsEmpty;

    /// <summary>
    /// Return a path that consists only of lower-case ASCII characters. Non-ASCII characters are left untouched.
    /// </summary>
    /// <remarks>The new string is not guaranteed to be a copy. If the original string was lower-case, no copy is created.</remarks>
    public Utf8GamePath ToLower()
        => new(Path.AsciiToLower());

    /// <summary>
    /// Create a new Utf8GamePath from a pointer.
    /// </summary>
    /// <param name="ptr">The data.</param>
    /// <param name="path">The resulting game path if successful, an empty path otherwise.</param>
    /// <param name="lower">Whether to turn the path into lower-case ASCII.</param>
    /// <returns>Whether the given pointer is a valid Utf8GamePath.</returns>
    public static unsafe bool FromPointer(byte* ptr, out Utf8GamePath path, bool lower = false)
    {
        var utf = new ByteString(ptr);
        return ReturnChecked(utf, out path, lower);
    }

    /// <summary>
    /// Same as <see cref="FromPointer"/> just with known length.
    /// </summary>
    public static bool FromSpan(ReadOnlySpan<byte> data, out Utf8GamePath path, bool lower = false)
    {
        var utf = ByteString.FromSpanUnsafe(data, false, null, null);
        return ReturnChecked(utf, out path, lower);
    }

    /// <summary>
    /// Does not check for Forward/Backslashes due to assuming that SE-strings use the correct one.
    /// Does not check for initial slashes either, since they are assumed to be by choice.
    /// Checks for maxlength and lowercase.
    /// </summary>
    /// <param name="utf"></param>
    /// <param name="path"></param>
    /// <param name="lower"></param>
    /// <returns></returns>
    private static bool ReturnChecked(ByteString utf, out Utf8GamePath path, bool lower = false)
    {
        path = Empty;
        if (utf.Length > MaxGamePathLength)
            return false;

        path = new Utf8GamePath(lower ? utf.AsciiToLower() : utf);
        return true;
    }

    /// <inheritdoc cref="ByteString.Clone"/>
    public Utf8GamePath Clone()
        => new(Path.Clone());

    /// <summary>
    /// Create a new path from a string.
    /// </summary>
    /// <param name="s">The given string.</param>
    /// <param name="path">The converted path or an empty path on failure.</param>
    /// <param name="toLower">Whether the string should be (ASCII) lower-cased.</param>
    /// <returns>False if the string is too long, or can not be converted to UTF8.</returns>
    public static bool FromString(string? s, out Utf8GamePath path, bool toLower = false)
    {
        path = Empty;
        if (string.IsNullOrEmpty(s))
            return true;

        var substring = s.Replace('\\', '/').TrimStart('/').Trim();
        if (substring.Length > MaxGamePathLength)
            return false;

        if (substring.Length == 0)
            return true;

        if (!ByteString.FromString(substring, out var ascii, toLower))
            return false;

        var a = new int[5, 5];
        path = new Utf8GamePath(ascii);
        return true;
    }

    /// <summary>
    /// Create a new path from a string and check its length.
    /// </summary>
    /// <param name="s">The given string.</param>
    /// <param name="path">The string as UTF8GamePath, empty string on failure or null input.</param>
    /// <param name="toLower">Whether the string should be (ASCII) lower-cased.</param>
    /// <returns>False if the string is too long.</returns>
    public static bool FromByteString(ByteString? s, out Utf8GamePath path, bool toLower = false)
    {
        if (s is null)
        {
            path = Empty;
            return true;
        }

        if (s.Length > MaxGamePathLength)
        {
            path = Empty;
            return false;
        }

        path = new Utf8GamePath(toLower ? s.AsciiToLower() : s);
        return true;
    }

    /// <summary>
    /// Create a new path from a file and a base directory.
    /// </summary>
    /// <param name="file">The file path to convert.</param>
    /// <param name="baseDir">The directory to which the file path should be seen as relative. </param>
    /// <param name="path">The converted path or an empty path on failure.</param>
    /// <param name="toLower">Whether the string should be (ASCII) lower-cased.</param>
    /// <returns>False if the file does not lie inside the base directory or the string conversion fails.</returns>
    public static bool FromFile(FileInfo file, DirectoryInfo baseDir, out Utf8GamePath path, bool toLower = false)
    {
        path = Empty;
        if (!file.FullName.StartsWith(baseDir.FullName))
            return false;

        var substring = file.FullName[(baseDir.FullName.Length + 1)..];
        return FromString(substring, out path, toLower);
    }

    /// <summary>
    /// Get the non-owned substring of the file name of the path.
    /// </summary>
    public ByteString Filename()
    {
        var idx = Path.LastIndexOf((byte)'/');
        return idx == -1 ? Path : Path.Substring(idx + 1);
    }

    /// <summary>
    /// Get the non-owned substring of the extension of the path.
    /// </summary>
    public ByteString Extension()
    {
        var idx = Path.LastIndexOf((byte)'.');
        return idx == -1 ? ByteString.Empty : Path.Substring(idx);
    }

    /// <inheritdoc cref="ByteString.Equals(ByteString?)"/>
    public bool Equals(Utf8GamePath other)
        => Path.Equals(other.Path);

    /// <inheritdoc cref="ByteString.GetHashCode"/>
    public override int GetHashCode()
        => Path.GetHashCode();

    /// <inheritdoc cref="ByteString.CompareTo"/>
    public int CompareTo(Utf8GamePath other)
        => Path.CompareTo(other.Path);

    /// <inheritdoc cref="ByteString.ToString"/>
    public override string ToString()
        => Path.ToString();

    /// <inheritdoc cref="ByteString.Dispose"/>
    public void Dispose()
        => Path.Dispose();

    /// <inheritdoc cref="IsRooted(ByteString)"/>
    public bool IsRooted()
        => IsRooted(Path);

    /// <summary>
    /// Return whether the path is rooted.
    /// </summary>
    public static bool IsRooted(ByteString path)
        => path.Length >= 1 && (path[0] == '/' || path[0] == '\\')
         || path.Length >= 2
         && (path[0] >= 'A' && path[0] <= 'Z' || path[0] >= 'a' && path[0] <= 'z')
         && path[1] == ':';

    /// <summary>
    /// Conversion from and to string.
    /// </summary>
    private class Utf8GamePathConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(Utf8GamePath);

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader).ToString();
            return FromString(token, out var p, true)
                ? p
                : throw new JsonException($"Could not convert \"{token}\" to {nameof(Utf8GamePath)}.");
        }

        public override bool CanWrite
            => true;

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is Utf8GamePath p)
                serializer.Serialize(writer, p.ToString());
        }
    }
}
