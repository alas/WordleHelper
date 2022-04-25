namespace WordleHelper.Code;

using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

internal class WordsLists
{
    private readonly string[] English;

    private readonly string[] Spanish;

    private readonly string[] Lunfardo;

    public WordsLists(string[] english, string[] spanish, string[] lunfardo)
    {
        English = english;
        Spanish = spanish;
        Lunfardo = lunfardo;
    }

    public static async Task<WordsLists> FactoryAsync()
    {
#if DEBUG
        var start = DateTime.UtcNow;
#endif

        #region Decompress Dictionaries

        var english = await FromGzipAsync(CompressedWordsLists.English);
        var spanish = await FromGzipAsync(CompressedWordsLists.Spanish);
        var lunfardo = await FromGzipAsync(CompressedWordsLists.Lunfardo);

        #endregion

#if DEBUG
        var dt = DateTime.UtcNow.Subtract(start).TotalSeconds;
        System.Diagnostics.Debug.WriteLine($"Decompressing dictionaries took {dt} seconds");
        start = DateTime.UtcNow;
#endif

        WordsLists result = new(english, spanish, lunfardo);

#if DEBUG
        dt = DateTime.UtcNow.Subtract(start).TotalSeconds;
        System.Diagnostics.Debug.WriteLine($"Processing dictionaries took {dt} seconds");
#endif
        return result;
    }

    public static async Task<string[]> FromGzipAsync(string value)
    {
        var bytes = Convert.FromBase64String(value);
        await using var input = new MemoryStream(bytes);
        await using var output = new MemoryStream();
        await using var stream = new GZipStream(input, CompressionMode.Decompress);

        await stream.CopyToAsync(output);
        await stream.FlushAsync();

        return Encoding.Unicode.GetString(output.ToArray())
            .Split("\n");
    }

    public string[] GetAllWords(Model model)
    {
        var words = model.Language switch
        {
            "Lunfardo" => Lunfardo,
            "Spanish" => Spanish,
            "English" or _ => English
        };

        if (!string.IsNullOrWhiteSpace(model.Pattern))
        {
            var pattern = model.Pattern.Trim().ToUpper();

            if (!pattern.StartsWith('^'))
                pattern = '^' + pattern;
            if (!pattern.EndsWith('$'))
                pattern = pattern + '$';

            var regexp = new Regex(pattern, RegexOptions.Compiled);

            words = words.Where(t => regexp.IsMatch(t)).ToArray();
        }

        if (!string.IsNullOrWhiteSpace(model.AvailableLetters))
        {
            var availableLetters = model.AvailableLetters.Trim().ToUpper();

            words = words.Where(t => t.All(c => availableLetters.Contains(c))).ToArray();
        }

        if (!string.IsNullOrWhiteSpace(model.RejectedLetters))
        {
            var rejectedLetters = model.RejectedLetters.Trim().ToUpper();

            words = words.Where(t => !t.Any(c => rejectedLetters.Contains(c))).ToArray();
        }

        if (!string.IsNullOrWhiteSpace(model.ObligatoryLetters))
        {
            var obligatoryLetters = model.ObligatoryLetters.Trim().ToUpper();

            words = words.Where(t => obligatoryLetters.All(c => t.Any(tc => tc == c))).ToArray();
        }

        return words;
    }
}
