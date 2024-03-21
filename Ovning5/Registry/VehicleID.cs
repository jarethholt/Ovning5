using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Ovning5.Tests")]

namespace Ovning5.Registry;

internal readonly struct VehicleIDHelper
{
    public const int LetterRangeStart = 65;
    public const int LetterRangeStop = 91;
    public const int NumberRangeStart = 48;
    public const int NumberRangeStop = 58;

    public static bool IsInNumberRange(int value)
        => NumberRangeStart <= value && value < NumberRangeStop;

    public static bool IsInLetterRange(int value)
        => LetterRangeStart <= value && value < LetterRangeStop;

    public static bool IsInRange(bool isAlpha, int value)
        => isAlpha ? IsInLetterRange(value) : IsInNumberRange(value);

    public static char GenerateLetter(Random random)
        => (char)random.Next(LetterRangeStart, LetterRangeStop);

    public static char GenerateNumber(Random random)
        => (char)random.Next(NumberRangeStart, NumberRangeStop);

    public static char GenerateCharacter(bool isAlpha, Random random)
        => isAlpha ? GenerateLetter(random) : GenerateNumber(random);

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

public readonly record struct VehicleID
{
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
        if (!VehicleIDHelper.MatchesFormat(code, _isAlpha))
            throw new FormatException(_formatErrorMessage);
        Code = code;
    }

    public override string ToString() => Code;

    public static VehicleID GenerateID(Random random)
    {
        char[] codeAsChars = new char[CodeLength];
        for (int i = 0; i < CodeLength; i++)
            codeAsChars[i]
                = VehicleIDHelper.GenerateCharacter(_isAlpha[i], random);
        return new VehicleID(new string(codeAsChars));
    }
}
