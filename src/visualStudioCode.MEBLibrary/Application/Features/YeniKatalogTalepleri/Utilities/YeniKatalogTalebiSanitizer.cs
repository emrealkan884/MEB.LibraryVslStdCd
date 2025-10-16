using System.Linq;

namespace Application.Features.YeniKatalogTalepleri.Utilities;

public static class YeniKatalogTalebiSanitizer
{
    public static string? NormalizeIsbn(string? isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            return null;

        string trimmed = isbn.Trim();
        var chars = trimmed.Where(char.IsLetterOrDigit).ToArray();
        if (chars.Length == 0)
            return null;

        return new string(chars).ToLowerInvariant();
    }
}