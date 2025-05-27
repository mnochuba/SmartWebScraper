namespace SmartWebScraper.Domain;
public static class Extensions
{
    /// <summary>
    /// Concatenates a collection of integers into a single string, separated by the specified separator.
    /// </summary>
    /// <param name="positions">Collection of strings to concatenate</param>
    /// <param name="separator">Separator to use for concatenation</param>
    /// <returns></returns>
    public static string Concatenate(this IEnumerable<int> positions, string separator = ",")
    {
        var concatenatedString = positions == null || !positions.Any()
            ? string.Empty
            : string.Join($"{separator} ", positions);
        return concatenatedString;
    }

    /// <summary>
    /// Concatenates a collection of strings into a single string, separated by the specified separator.
    /// </summary>
    /// <param name="positions">Collection of strings to concatenate</param>
    /// <param name="separator">Separator to use for concatenation</param>
    /// <returns></returns>
    public static string Concatenate(this IEnumerable<string> positions, string separator = ",")
    {
        var concatenatedString = positions == null || !positions.Any()
            ? string.Empty
            : string.Join($"{separator} ", positions);
        return concatenatedString;
    }

    /// <summary>
    /// Splits a concatenated string into a collection of integers, using the specified separator.
    /// </summary>
    /// <param name="concatenatedString">String to split</param>
    /// <param name="separatorChar">Specified separator character</param>
    /// <returns></returns>
    public static IEnumerable<int> SplitIntoCollection(this string concatenatedString, char separatorChar = ',')
    {
        return string.IsNullOrWhiteSpace(concatenatedString) ? []
            : concatenatedString.Split([separatorChar], StringSplitOptions.RemoveEmptyEntries)
                                .Select(item => int.TryParse(item.Trim(), out var num) ? num : (int?)null)
                                .Where(num => num.HasValue)
                                .Select(num => num!.Value)
                                .ToList();
    }

    /// <summary>
    /// Splits a concatenated string into a collection of strings, using the specified separator.
    /// </summary>
    /// <param name="concatenatedString">String to split</param>
    /// <param name="separatorChar">Specified separator character</param>
    /// <returns></returns>
    public static IEnumerable<string> SplitIntoStringCollection(this string concatenatedString, char separatorChar = ',')
    {
        return string.IsNullOrWhiteSpace(concatenatedString) ? []
            : concatenatedString.Split([separatorChar], StringSplitOptions.RemoveEmptyEntries)
                                .Select(item => item.Trim())
                                .ToList();
    }
}
