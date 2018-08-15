using System.Linq;

namespace TVShowChecker.Infrastructure
{
    static class StringExtensions
    {
        public static bool IsNumeric(this string a)
        {
            return !string.IsNullOrWhiteSpace(a) && a.All(char.IsDigit);
        }
    }
}
