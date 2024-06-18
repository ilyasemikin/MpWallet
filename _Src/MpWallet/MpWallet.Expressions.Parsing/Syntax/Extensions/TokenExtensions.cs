using System.Diagnostics.CodeAnalysis;

namespace MpWallet.Expressions.Parsing.Syntax.Extensions;

public static class TokenExtensions
{
    public static Token Trim(this Token token)
    {
        return token.TryTrim(out var result)
            ? result
            : throw new ArgumentException("Trimmed value is empty", nameof(token));
    }

    public static bool TryTrim(this Token token, [NotNullWhen(true)] out Token? result)
    {
        var trimmed = token.Value.TrimStart();
        var begin = token.Begin + (token.Value.Length - trimmed.Length);

        trimmed = trimmed.TrimEnd();

        if (trimmed.Length == 0)
        {
            result = null;
            return false;
        }

        result = new Token(token.Input, begin, begin + trimmed.Length);
        return true;
    }
}