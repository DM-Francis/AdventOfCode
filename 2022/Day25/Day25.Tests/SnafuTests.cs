namespace Day25.Tests;

public class SnafuTests
{
    [Theory]
    [InlineData("=", "=", "-1")]
    [InlineData("=", "-", "-2")]
    [InlineData("=", "0", "=")]
    [InlineData("=", "1", "-")]
    [InlineData("1-", "12", "21")]
    [InlineData("10", "2", "12")]
    [InlineData("22", "1", "1==")]
    public void CanAddSnafuStrings(string a, string b, string expected)
    {
        var actual = Snafu.Add(a, b);
        Assert.Equal(expected, actual);
    }
}