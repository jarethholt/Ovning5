using Ovning5.Vehicles;

namespace Ovning5.Tests.RegistryTests;

public class VehicleIDTests
{
    private const int _seed = 12345;
    private static readonly string[] _codes = ["BBU578", "ETG523", "FAD718"];

    [Fact]
    public void GenerateID_Seeded_Equals()
    {
        Random random = new(_seed);
        VehicleID[] actual = new VehicleID[_codes.Length];
        for (int i = 0; i < _codes.Length; i++)
        {
            actual[i] = VehicleID.GenerateID(random);
            Assert.Equal(_codes[i], actual[i].ToString());
        }
    }

    [Fact]
    public void VehicleID_ValidInput_OK()
    {
        Exception? exception = Record.Exception(
            () => _ = new VehicleID("ABC123")
        );
        Assert.Null(exception);
    }

    [Fact]
    public void VehicleID_InvalidInput_ThrowsFormatException()
    {
        string[] codes = ["ABC12", "XYZ0987", "", "ABc123"];
        foreach (string code in codes)
        {
            void test() => _ = new VehicleID(code);
            Assert.Throws<FormatException>(test);
        }
    }
}
