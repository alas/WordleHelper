namespace WordListsGenerator;

using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

internal static class WordList
{
    public static string GetAllWordsEnglish() => GetAllWords(@"WordLists\English", false);
    public static string GetAllWordsSpanish() => GetAllWords(@"WordLists\Spanish", true);

    private static string GetAllWords(string path, bool isSpanish)
    {
        var filePaths = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        StringBuilder sb = new();
        foreach (var filePath in filePaths)
        {
            sb.Append(File.ReadAllText(filePath));
        }
        var allwords = sb.ToString();
        var allwordsfiltered = FilterWords(allwords, isSpanish);
        return allwordsfiltered;
    }

    private static string FilterWords(string input, bool isSpanish)
    {
        if (isSpanish)
        {
            input = input
                .Replace('á', 'a')
                .Replace('é', 'e')
                .Replace('í', 'i')
                .Replace('ó', 'o')
                .Replace('ú', 'u')
                .Replace('ü', 'u')
                .Replace('Á', 'A')
                .Replace('É', 'E')
                .Replace('Í', 'I')
                .Replace('Ó', 'O')
                .Replace('Ú', 'U')
                .Replace('Ü', 'U');
        }

        Regex regexp = new(@"^[A-ZÑ]+$", RegexOptions.Compiled);

        var words = input.Split("\n")
            .Select(t => t.Trim().ToUpper())
            .Where(t => new[] { 5, 6, 7 }.Contains(t.Length))
            .Where(t => regexp.IsMatch(t))
            .Distinct()
            .OrderBy(t => t)
            .ToArray();

        return String.Join("\n", words);
    }
}
