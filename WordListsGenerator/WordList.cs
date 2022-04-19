namespace WordListsGenerator;

using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

internal static class WordList
{
    public static string GetAllWordsEnglish() => GetAllWords(@"WordLists\English", false, true, false);
    public static string GetAllWordsSpanish() => GetAllWords(@"WordLists\Spanish", true, true, false);
    public static string GetAllWordsSpanishFull() => GetAllWords(@"WordLists\Spanish", true, false, false);
    public static string GetAllWordsLunfardo() => GetAllWords(@"WordLists\Lunfardo", true, false, true);

    private static string GetAllWords(string path, bool isSpanish, bool filterLength567, bool shuffle)
    {
        var filePaths = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        StringBuilder sb = new();
        foreach (var filePath in filePaths)
        {
            sb.Append(File.ReadAllText(filePath));
        }
        var allwords = sb.ToString();
        var allwordsfiltered = FilterWords(allwords, isSpanish, filterLength567, shuffle);
        return allwordsfiltered;
    }

    private static string FilterWords(string input, bool isSpanish, bool filterLength567, bool shuffle)
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

            Regex regexpComma = new(@"^.*[,] .*$", RegexOptions.Compiled);

            var tempWords = input.Split("\n")
                .Select(t => t.TrimEnd('1', '2', '3'))
                .Select(t => !regexpComma.IsMatch(t) ? t : Regex.Replace(t, @"[,] .*$", "", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(0.5)))
                .ToArray();

            input = String.Join("\n", tempWords);
        }

        Regex regexp = new(@"^[A-ZÑ]+$", RegexOptions.Compiled);

        var filter = filterLength567 ? new[] { 5, 6, 7 } : new[] { 5, 6, 7, 8, 9, 10 };

        var words = input.Split("\n")
            .Select(t => t.Trim().ToUpper())
            .Where(t => filter.Contains(t.Length))
            .Where(t => regexp.IsMatch(t))
            .Distinct()
            .OrderBy(t => t)
            .ToArray();

        if (shuffle)
        {
            Random rnd = new(354);
            words = words.OrderBy(x => rnd.Next()).ToArray();
        }

        return String.Join("\n", words);
    }
}
