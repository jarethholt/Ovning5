using Ovning5.Vehicles;

namespace Ovning5.Tests.RegistryTests;

public class VehicleIDHelperTests
{
    private static readonly int _seed = 12345;
    private static readonly string _letters = "BBUNUVETGN";
    private static readonly string _numbers = "0075781725";
    private static readonly bool[] _isAlpha
        = [true, true, true, false, false, false];

    [Fact]
    public static void GenerateLetter_Seeded_Equals()
    {
        Random random = new(_seed);
        char[] chars = new char[_letters.Length];
        for (int i = 0; i < chars.Length; i++)
            chars[i] = VehicleIDHelper.GenerateLetter(random);
        string expected = new(chars);
        Assert.Equal(expected, _letters);
    }

    [Fact]
    public static void GenerateNumber_Seeded_Equals()
    {
        Random random = new(_seed);
        char[] chars = new char[_numbers.Length];
        for (int i = 0; i < chars.Length; i++)
            chars[i] = VehicleIDHelper.GenerateNumber(random);
        string expected = new(chars);
        Assert.Equal(expected, _numbers);
    }

    [Fact]
    public static void GenerateCharacter_Seeded_Equals()
    {
        Random random = new(_seed);
        char[] actual = new char[_isAlpha.Length];
        char[] expected = new char[_isAlpha.Length];
        for (int i = 0; i < _isAlpha.Length; i++)
        {
            expected[i] = _isAlpha[i] ? _letters[i] : _numbers[i];
            actual[i] = VehicleIDHelper.GenerateCharacter(_isAlpha[i], random);
        }
        Assert.Equal(new string(expected), new string(actual));
    }

    [Fact]
    public static void MatchesFormat_Valid_True()
    {
        string[] codes = ["ABC123", "XYZ098", "DEF782"];
        foreach (string code in codes)
            Assert.True(VehicleIDHelper.MatchesFormat(code, _isAlpha));
    }

    [Fact]
    public static void MatchesFormat_WrongLength_False()
    {
        string[] codes = ["ABC12", "XYZ0987", ""];
        foreach (string code in codes)
            Assert.False(VehicleIDHelper.MatchesFormat(code, _isAlpha));
    }

    [Fact]
    public static void MatchesFormat_Lowercase_False()
    {
        Assert.False(VehicleIDHelper.MatchesFormat("abc123", _isAlpha));
    }

    [Fact]
    public static void MatchesFormat_NonLatin_False()
    {
        Assert.False(VehicleIDHelper.MatchesFormat("ÄÖÅ123", _isAlpha));
    }
}
