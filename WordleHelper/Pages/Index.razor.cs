namespace WordleHelper.Pages;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.RegularExpressions;

public partial class Index
{
    [Inject] HttpClient Http { get; set; }

    private bool Loaded = false;

    private bool LoadingResults = false;

    private Model Model { get; set; } = new();

    private Dictionary Dictionary;

    private string[]? Results;

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
    public string? ObligatoryLetters;
    public string? Pattern;
}

/*
 * dictionaries from https://www.aspnetspell.com/Downloads
 */
public class Dictionary
{
    private readonly string[] English;

    private readonly string[] Spanish;

    public Dictionary(string english, string spanish)
    {
        English = ProcessFile(english);
        Spanish = ProcessFile(spanish);
    }

    // todo: reduce execution time for this function
    private static string[] ProcessFile(string file)
    {
        var regexp = new Regex(@"^[a-z]+$");

        var words = file
            .Split(Environment.NewLine)
            .Select(t => t.Trim().ToLower())
            .Where(t => new[] { 5, 6 }.Contains(t.Length))
            .Where(t => regexp.IsMatch(t))
            .ToArray();

        return words;
    }

    public string[] GetAllWords(Model model)
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

            var regexp = new Regex(pattern);

            words = words.Where(t => regexp.IsMatch(t)).ToArray();
        }

        if (!string.IsNullOrWhiteSpace(model.AvailableLetters))
        {
            var availableLetters = model.AvailableLetters.Trim().ToLower();

            words = words.Where(t => t.All(c => availableLetters.Contains(c))).ToArray();
        }

        if (!string.IsNullOrWhiteSpace(model.ObligatoryLetters))
        {
            var obligatoryLetters = model.ObligatoryLetters.Trim().ToLower();

            words = words.Where(t => obligatoryLetters.All(c => t.Any(tc => tc == c))).ToArray();
        }

        return words;
    }
}
