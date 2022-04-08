namespace WordleHelper.Code;

internal class Model
{
    public readonly string[] LanguageOptions = { "English", "Spanish" };
    public string? Language;
    public string? AvailableLetters;
    public string? RejectedLetters;
    public string? ObligatoryLetters;
    public string? Pattern;
}
