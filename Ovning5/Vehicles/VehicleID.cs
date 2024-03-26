using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("Ovning5.Tests")]

namespace Ovning5.Vehicles;

internal readonly struct VehicleIDHelper
{
    // Character number ranges for A-Z and 0-9
    public const int LetterRangeStart = 65;
    public const int LetterRangeStop = 91;
    public const int NumberRangeStart = 48;
    public const int NumberRangeStop = 58;

    // Check whether an integer is in either range
    public static bool IsInNumberRange(int value)
        => NumberRangeStart <= value && value < NumberRangeStop;

    public static bool IsInLetterRange(int value)
        => LetterRangeStart <= value && value < LetterRangeStop;

    public static bool IsInRange(bool isAlpha, int value)
        => isAlpha ? IsInLetterRange(value) : IsInNumberRange(value);

    // Generate a random character in one of the ranges
    public static char GenerateLetter(Random random)
        => (char)random.Next(LetterRangeStart, LetterRangeStop);

    public static char GenerateNumber(Random random)
        => (char)random.Next(NumberRangeStart, NumberRangeStop);

    public static char GenerateCharacter(bool isAlpha, Random random)
        => isAlpha ? GenerateLetter(random) : GenerateNumber(random);

    // Check whether a string matches the given code format
    // given by isAlpha (true for A-Z, false for 0-9)
    public static bool MatchesFormat(string code, bool[] isAlpha)
    {
        int codeLength = isAlpha.Length;
        if (code.Length != codeLength)
            return false;

        for (int i = 0; i < codeLength; i++)
        {
            bool inRange = IsInRange(isAlpha[i], code[i]);
            if (!inRange)
                return false;
        }
        return true;
    }
}

// Register a JSON converter for automatic serialization
[JsonConverter(typeof(VehicleIDJsonConverter))]
internal readonly record struct VehicleID
{
    // VehicleID format: AAA111
    private static readonly bool[] _isAlpha
        = [true, true, true, false, false, false];
    public static readonly string CodeFormat
        = new(_isAlpha.Select(isAlpha => isAlpha ? 'A' : '1').ToArray());
    public static readonly int CodeLength = _isAlpha.Length;
    private static readonly string _formatErrorMessage
        = $"The format of a registration code should be {CodeFormat}";

    public readonly string Code { get; }

    public VehicleID(string code)
    {
        if (!Validate(code))
            throw new FormatException(_formatErrorMessage);
        Code = code;
    }

    public override string ToString() => Code;

    public bool Equals(string code) => Code == code;

    public static bool Validate(string code)
        => VehicleIDHelper.MatchesFormat(code, _isAlpha);

    // Generate a random VehicleID
    public static VehicleID GenerateID(Random random)
    {
        char[] codeAsChars = new char[CodeLength];
        for (int i = 0; i < CodeLength; i++)
            codeAsChars[i]
                = VehicleIDHelper.GenerateCharacter(_isAlpha[i], random);
        return new VehicleID(new string(codeAsChars));
    }

    // Generate a random VehicleID not present in a given list
    public static VehicleID GenerateUniqueID(
        Random random,
        IEnumerable<VehicleID> vehicleIDs)
    {
        VehicleID vehicleID;
        do
        {
            vehicleID = GenerateID(random);
            if (!vehicleIDs.Contains(vehicleID))
                break;
        } while (true);
        return vehicleID;
    }
}

// Custom JSON converter to only serialize based on VehicleID.Code
internal class VehicleIDJsonConverter : JsonConverter<VehicleID>
{
    public override VehicleID Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    ) => new(reader.GetString()!);

    public override void Write(
        Utf8JsonWriter writer,
        VehicleID value,
        JsonSerializerOptions options
    ) => writer.WriteStringValue(value.Code);
}
