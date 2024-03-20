namespace Ovning5.Registration;

public readonly struct RegistrationCodeHelper
{
    public const int LetterRangeStart = 65;
    public const int LetterRangeStop = 91;
    public const int LetterRangeSize = 26;
    public const int NumberRangeStart = 48;
    public const int NumberRangeStop = 58;
    public const int NumberRangeSize = 10;

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

public readonly struct RegistrationCode : IEquatable<RegistrationCode>
{
    private static readonly bool[] _isAlpha
        = [true, true, true, false, false, false];
    public static readonly string CodeFormat
        = new(_isAlpha.Select(isAlpha => isAlpha ? 'A' : '1').ToArray());
    public static readonly int CodeLength = _isAlpha.Length;
    private static readonly string _formatErrorMessage
        = $"The format of a registration code should be {CodeFormat}";

    public readonly string Code { get; init; }

    public RegistrationCode(string code)
    {
        if (!RegistrationCodeHelper.MatchesFormat(code, _isAlpha))
            throw new FormatException(_formatErrorMessage);
        this.Code = code;
    }

    public override string ToString() => Code;

    public bool Equals(RegistrationCode other) => this.Code == other.Code;

    public override bool Equals(object? obj)
        => obj is RegistrationCode other && Equals(other);

    public override int GetHashCode() => this.Code.GetHashCode();

    public static bool operator ==(RegistrationCode left, RegistrationCode right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(RegistrationCode left, RegistrationCode right)
    {
        return !(left == right);
    }

    public static RegistrationCode GenerateCode(Random random)
    {
        char[] codeAsChars = new char[CodeLength];
        for (int i = 0; i < CodeLength; i++)
            codeAsChars[i]
                = RegistrationCodeHelper.GenerateCharacter(_isAlpha[i], random);
        return new RegistrationCode(new string(codeAsChars));
    }
}
