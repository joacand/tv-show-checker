using System.Linq;

namespace TVShowChecker.Core.Extensions;

public static class StringExtensions
{
    public static bool IsNumeric(this string a)
    {
        return !string.IsNullOrWhiteSpace(a) && a.All(char.IsDigit);
    }
}
