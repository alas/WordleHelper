namespace WordListsGenerator;

using System.IO.Compression;
using System.Text;

internal static class Compress
{
    /// <param name="value"></param>
    /// <param name="level">Optimal generates the smallest files not counting brotli that is not supported on platform: browser</param>
    public static async Task<string> ToGzipAsync(this string value, CompressionLevel level = CompressionLevel.Optimal)
    {
        var bytes = Encoding.Unicode.GetBytes(value);
        await using var input = new MemoryStream(bytes);
        await using var output = new MemoryStream();
        await using var stream = new GZipStream(output, level);

        await input.CopyToAsync(stream);
        await stream.FlushAsync();
        await output.FlushAsync();

        var result = output.ToArray();

        return Convert.ToBase64String(result);
    }
}
