namespace Penumbra.String.Functions;

public static unsafe partial class ByteStringFunctions
{
    /// <summary> Compute the FFXIV-CRC64 value of a UTF16 string. </summary>
    /// <remarks>
    /// The FFXIV-CRC64 consists of the CRC32 of the string up to the last '/' in the lower bytes,
    /// and the CRC32 of the string from the last '/' in the upper bytes.
    /// </remarks>
    public static ulong ComputeCrc64(string name)
    {
        if (name.Length == 0)
            return 0;

        var lastSlash = name.LastIndexOf('/');
        if (lastSlash == -1)
            return Lumina.Misc.Crc32.Get(name);

        var folder = name[..lastSlash];
        var file   = name[(lastSlash + 1)..];
        return ((ulong)Lumina.Misc.Crc32.Get(folder) << 32) | Lumina.Misc.Crc32.Get(file);
    }

    /// <summary> Compute the FFXIV-CRC64 value of a UTF8 string. </summary>
    /// <remarks><inheritdoc cref="ComputeCrc64(string)" /></remarks>
    public static ulong ComputeCrc64(ReadOnlySpan<byte> name)
    {
        if (name.Length == 0)
            return 0;

        var lastSlash = name.LastIndexOf((byte)'/');
        if (lastSlash == -1)
            return Lumina.Misc.Crc32.Get(name);

        var folder = name[..lastSlash];
        var file   = name[(lastSlash + 1)..];
        return ((ulong)Lumina.Misc.Crc32.Get(folder) << 32) | Lumina.Misc.Crc32.Get(file);
    }

    private static readonly uint[] CrcTable =
        typeof(Lumina.Misc.Crc32).GetField("CrcTable", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null) as uint[]
     ?? throw new Exception("Could not fetch CrcTable from Lumina.");


    /// <summary>
    /// Compute the FFXIV-CRC64 value, the CRC32 value, the ASCII state and the Lowercase state while iterating only once.
    /// </summary>
    public static int ComputeCrc64LowerAndSize(byte* ptr, out ulong crc64, out int crc32Ret, out bool isLower, out bool isAscii)
    {
        var  tmp       = ptr;
        uint crcFolder = 0;
        uint crcFile   = 0;
        var  crc32     = uint.MaxValue;
        crc64   = 0;
        isLower = true;
        isAscii = true;
        while (true)
        {
            var value = *tmp;
            if (value == 0)
                break;

            if (AsciiToLower(*tmp) != *tmp)
                isLower = false;

            if (value > 0x80)
                isAscii = false;

            if (value == (byte)'/')
            {
                crcFolder = crc32;
                crcFile   = uint.MaxValue;
                crc32     = CrcTable[(byte)(crc32 ^ value)] ^ (crc32 >> 8);
            }
            else
            {
                crcFile = CrcTable[(byte)(crcFolder ^ value)] ^ (crcFolder >> 8);
                crc32   = CrcTable[(byte)(crc32 ^ value)] ^ (crc32 >> 8);
            }

            ++tmp;
        }

        var size = (int)(tmp - ptr);
        crc64    = ~((ulong)crcFolder << 32) | crcFile;
        crc32Ret = (int)~crc32;
        return size;
    }

    /// <summary>
    /// Compute the CRC32 value, the ASCII state and the Lowercase state while iterating only once.
    /// </summary>
    public static int ComputeCrc32AsciiLowerAndSize(byte* ptr, out int crc32Ret, out bool isLower, out bool isAscii)
    {
        var tmp   = ptr;
        var crc32 = uint.MaxValue;
        isLower = true;
        isAscii = true;
        while (true)
        {
            var value = *tmp;
            if (value == 0)
                break;

            if (AsciiToLower(*tmp) != *tmp)
                isLower = false;

            if (value > 0x80)
                isAscii = false;

            crc32 = CrcTable[(byte)(crc32 ^ value)] ^ (crc32 >> 8);
            ++tmp;
        }

        var size = (int)(tmp - ptr);
        crc32Ret = (int)~crc32;
        return size;
    }
}
