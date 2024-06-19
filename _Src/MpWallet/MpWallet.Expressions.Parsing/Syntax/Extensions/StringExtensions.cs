namespace MpWallet.Expressions.Parsing.Syntax.Extensions;

public static class StringExtensions
{
    public static Token ToToken(this string input)
    {
        return new Token(input, 0, input.Length);
    }
    
    public static Token ToToken(this string input, int begin, int end)
    {
        return new Token(input, begin, end);
    }
}