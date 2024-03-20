using Ovning5.Registration;

namespace Ovning5.Tests.RegistrationTests;

public class RegistrationCodeTests
{
    private const int _seed = 12345;
    private static readonly string[] _codes = ["BBU578", "ETG523", "FAD718"];

    [Fact]
    public void GenerateCode_Seeded_Matches()
    {
        Random random = new(_seed);
        RegistrationCode[] actual = new RegistrationCode[_codes.Length];
        for (int i = 0; i < _codes.Length; i++)
        {
            actual[i] = RegistrationCode.GenerateCode(random);
            Assert.Equal(_codes[i], actual[i].ToString());
        }
    }
}
