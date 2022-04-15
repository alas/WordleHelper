namespace WordListsGenerator;

using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

internal static class WordList
{
    public static string GetAllWordsEnglish() => GetAllWords(@"WordLists\English", false, true, true);
    public static string GetAllWordsSpanish() => GetAllWords(@"WordLists\Spanish", true, true, true);
    public static string GetAllWordsSpanishFull() => GetAllWords(@"WordLists\Spanish", true, false, true);
    public static string GetAllWordsLunfardo() => GetAllWords(@"WordLists\Lunfardo", true, false, false);

    private static string GetAllWords(string path, bool isSpanish, bool filterLength, bool sort)
    {
        var filePaths = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        StringBuilder sb = new();
        foreach (var filePath in filePaths)
        {
            sb.Append(File.ReadAllText(filePath));
        }
        var allwords = sb.ToString();
        var allwordsfiltered = FilterWords(allwords, isSpanish, filterLength, sort);
        return allwordsfiltered;
    }

    private static string FilterWords(string input, bool isSpanish, bool filterLength, bool sort)
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
            .Where(t => !filterLength || new[] { 5, 6, 7 }.Contains(t.Length))
            .Where(t => regexp.IsMatch(t))
            .Distinct()
            .ToArray();

        if (sort)
            Array.Sort(words);

        return String.Join("\n", words);
    }
}
