namespace WordListsGenerator;

using ICSharpCode.SharpZipLib.BZip2;
using System.IO.Compression;
using System.Text;

internal static class Compress
{
    public static async Task<string> ToBZip2Async(this string value, int level = 4096)
    {
        var bytes = Encoding.Unicode.GetBytes(value);
        await using var input = new MemoryStream(bytes);
        await using var output = new MemoryStream();
        BZip2.Compress(input, output, true, level);
        var result = output.ToArray();
        return Convert.ToBase64String(result);
    }

    public static async Task<string> FromBZip2Async(this string value)
    {
        var bytes = Convert.FromBase64String(value);
        await using var input = new MemoryStream(bytes);
        await using var output = new MemoryStream();
        BZip2.Decompress(input, output, true);
        return Encoding.Unicode.GetString(output.ToArray());
    }

    public static async Task<string> ToGzipAsync(this string value, CompressionLevel level = CompressionLevel.SmallestSize)
    {
        var bytes = Encoding.Unicode.GetBytes(value);
        await using var input = new MemoryStream(bytes);
        await using var output = new MemoryStream();
        await using var stream = new GZipStream(output, level);

        await input.CopyToAsync(stream);

        var result = output.ToArray();

        return Convert.ToBase64String(result);
    }

    public static async Task<string> FromGzipAsync(this string value)
    {
        var bytes = Convert.FromBase64String(value);
        await using var input = new MemoryStream(bytes);
        await using var output = new MemoryStream();
        await using var stream = new GZipStream(input, CompressionMode.Decompress);

        await stream.CopyToAsync(output);
        await stream.FlushAsync();

        return Encoding.Unicode.GetString(output.ToArray());
    }
}
