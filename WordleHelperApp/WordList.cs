namespace WordleHelperApp;

using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

internal static class WordList
{
    public static string GetAllWordsEnglish() => GetAllWords(@"WordLists\English");
    public static string GetAllWordsSpanish() => GetAllWords(@"WordLists\Spanish");

    private static string GetAllWords(string path)
    {
        var filePaths = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        StringBuilder sb = new();
        foreach (var filePath in filePaths)
        {
            sb.Append(File.ReadAllText(filePath));
        }
        var allwords = sb.ToString();
        var allwordsfiltered = FilterWords(allwords);
        return allwordsfiltered;
    }

    private static string FilterWords(string input)
    {
        Regex regexp = new(@"^[A-ZÑ]+$", RegexOptions.Compiled);

        var words = input
            .Split("\n")
            .Select(t => t.Trim().ToUpper())
            .Where(t => new[] { 5, 6, 7 }.Contains(t.Length))
            .Where(t => regexp.IsMatch(t))
            .Distinct()
            .OrderBy(t => t)
            .ToArray();

        return String.Join("\n", words);
    }
}
