namespace WordleHelper.Pages;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

public partial class Index
{
    [Inject] HttpClient Http { get; set; }

    private bool Loaded = false;

    private bool LoadingResults = false;

    private Model Model { get; set; } = new();

    private Dictionary Dictionary;

    private List<string>? Results;

    protected override async Task OnInitializedAsync()
    {
        var file1 = await Http.GetStringAsync("data/English (International).dic");
        var file2 = await Http.GetStringAsync("data/Espanol.dic");
        Dictionary = new(file1, file2);
        Loaded = true;
    }

    private void FormSubmitted(EditContext editContext)
    {
        LoadingResults = true;
        try
        {
            Results = Dictionary.GetAllWords(Model);
        }
        finally
        {
            LoadingResults = false;
        }
    }
}

public class Model
{
    public readonly string[] LanguageOptions = { "English", "Spanish" };
    public string? Language;
    public string? AvailableLetters;
    public string? RejectedLetters;
    public string? ObligatoryLetters;
    public string? Pattern;
}

/*
 * dictionaries from https://www.aspnetspell.com/Downloads
 */
public class Dictionary
{
    private readonly List<string> English;

    private readonly List<string> Spanish;

    public Dictionary(string english, string spanish)
    {
        var start = DateTime.UtcNow;
        English = ProcessFile(english);
        Spanish = ProcessFile(spanish);
        var dt = DateTime.UtcNow.Subtract(start).TotalSeconds;
    }

    private static List<string> ProcessFile(string file)
    {
        List<string> results = new();

        foreach (var word in file.SplitLines())
        {
            if (word.Length == 5 || word.Length == 6 || word.Length == 7)
            {
                var isValid = true;
                for (var i = 0; i < word.Length && isValid; i++)
                {
                    if (!char.IsLetter(word[i]))
                    {
                        isValid = false;
                    }
                }                
                if (isValid) results.Add(word.ToString());
            }
        }

        return results;
    }

    public List<string> GetAllWords(Model model)
    {
        var words = model.Language switch
        {
            "Spanish" => Spanish,
            "English" or _ => English
        };

        if (!string.IsNullOrWhiteSpace(model.Pattern))
        {
            var pattern = model.Pattern.Trim().ToLower();

            if (!pattern.StartsWith('^'))
                pattern = '^' + pattern;
            if (!pattern.EndsWith('$'))
                pattern = pattern + '$';

            var regexp = new Regex(pattern, RegexOptions.Compiled);

            words = words.Where(t => regexp.IsMatch(t)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(model.AvailableLetters))
        {
            var availableLetters = model.AvailableLetters.Trim().ToLower();

            words = words.Where(t => t.All(c => availableLetters.Contains(c))).ToList();
        }

        if (!string.IsNullOrWhiteSpace(model.RejectedLetters))
        {
            var rejectedLetters = model.RejectedLetters.Trim().ToLower();

            words = words.Where(t => !t.Any(c => rejectedLetters.Contains(c))).ToList();
        }

        if (!string.IsNullOrWhiteSpace(model.ObligatoryLetters))
        {
            var obligatoryLetters = model.ObligatoryLetters.Trim().ToLower();

            words = words.Where(t => obligatoryLetters.All(c => t.Any(tc => tc == c))).ToList();
        }

        return words.Distinct().OrderBy(t => t).ToList();
    }
}

// from https://www.meziantou.net/split-a-string-into-lines-without-allocation.htm
public static class StringExtensions
{
    public static LineSplitEnumerator SplitLines(this string str)
    {
        // LineSplitEnumerator is a struct so there is no allocation here
        return new LineSplitEnumerator(str.AsSpan());
    }

    // Must be a ref struct as it contains a ReadOnlySpan<char>
    public ref struct LineSplitEnumerator
    {
        private ReadOnlySpan<char> _str;

        public LineSplitEnumerator(ReadOnlySpan<char> str)
        {
            _str = str;
            Current = default;
        }

        // Needed to be compatible with the foreach operator
        public LineSplitEnumerator GetEnumerator() => this;

        public bool MoveNext()
        {
            var span = _str;
            if (span.Length == 0) // Reach the end of the string
                return false;

            var index = span.IndexOfAny('\r', '\n');
            if (index == -1) // The string is composed of only one line
            {
                _str = ReadOnlySpan<char>.Empty; // The remaining string is an empty string
                Current = span;
                return true;
            }

            if (index < span.Length - 1 && span[index] == '\r')
            {
                // Try to consume the '\n' associated to the '\r'
                var next = span[index + 1];
                if (next == '\n')
                {
                    Current = span[..index];
                    _str = span[(index + 2)..];
                    return true;
                }
            }

            Current = span[..index];
            _str = span[(index + 1)..];
            return true;
        }

        public ReadOnlySpan<char> Current { get; private set; }
    }
}
