using System.Globalization;
using MpWallet.Values.Implementations;

namespace MpWallet.Values.UnitTests;

public sealed class NumberTests
{
    [Fact]
    public void Constructor_ShouldCorrectCreate()
    {
        var value = 1.5M;

        var number = new Number(value);

        Assert.Equal(value, number.Value);
    }

    [Theory]
    [InlineData([1])]
    [InlineData([1.5])]
    public void TryParse_ShouldParse_WhenInputCorrect(double value)
    {
        var decimalValue = new decimal(value);
        var expected = new Number(decimalValue);

        var input = decimalValue.ToString(CultureInfo.InvariantCulture);
        var result = Number.TryParse(input, out var actual);
        
        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1234fdas")]
    public void TryParse_ShouldFailure_WhenInputIncorrect(string input)
    {
        var result = Number.TryParse(input, out var actual);
        
        Assert.False(result);
        Assert.Null(actual);
    }

    [Theory]
    [InlineData([1, "en-US"])]
    [InlineData([1.5, "en-US"])]
    [InlineData([1, "ru-RU"])]
    [InlineData([1.5, "ru-RU"])]
    public void TryParse_ShouldParse_WhenFormatProviderPassed(double value, string culture)
    {
        var decimalValue = new decimal(value);
        var formatProvider = new CultureInfo(culture);
        var expected = new Number(decimalValue);

        var input = decimalValue.ToString(formatProvider);
        var result = Number.TryParse(input, formatProvider, out var actual);

        Assert.True(result);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("1.5", "ru-RU")]
    [InlineData("dsaf", "en-US")]
    public void TryParse_ShouldFailure_WhenInputIncorrectAndFormatProviderPassed(string input, string culture)
    {
        var formatProvider = new CultureInfo(culture);
            
        var result = Number.TryParse(input, formatProvider, out var actual);
        Assert.False(result);
        Assert.Null(actual);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(1.5)]
    public void ToString_ShouldCorrect(double value)
    {
        var decimalValue = new decimal(value);
        var expected = decimalValue.ToString(CultureInfo.InvariantCulture);

        var number = new Number(decimalValue);

        var actual = number.ToString();
        
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(1, "en-US")]
    [InlineData(1.5, "en-US")]
    [InlineData(1, "ru-RU")]
    [InlineData(1.5, "ru-RU")]
    public void ToString_ShouldCorrect_WhenFormatProviderPassed(double value, string culture)
    {
        var decimalValue = new decimal(value);
        var formatProvider = new CultureInfo(culture);
        var expected = value.ToString(formatProvider);

        var number = new Number(decimalValue);

        var actual = number.ToString(string.Empty, formatProvider);
        
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Min_ShouldValid()
    {
        Assert.Equal(decimal.MinValue, Number.Min);
    }

    [Fact]
    public void Max_ShouldValid()
    {
        Assert.Equal(decimal.MaxValue, Number.Max);
    }
}
