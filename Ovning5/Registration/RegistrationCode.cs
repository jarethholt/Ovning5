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
}

public readonly struct RegistrationCode : IEquatable<RegistrationCode>
{
    private static readonly bool[] _charIsAlpha
        = [true, true, true, false, false, false];
    public static readonly string CodeFormat
        = new(_charIsAlpha.Select(isAlpha => isAlpha ? 'A' : '1').ToArray());
    public static readonly int CodeLength = _charIsAlpha.Length;
    private static readonly string _formatErrorMessage
        = $"The format of a registration code should be {CodeFormat}";

    private readonly int[] CodeAsInts { get; init; }
    private readonly char[] CodeAsChars { get; init; }
    public readonly string Code { get; init; }

    public RegistrationCode(int[] codeAsInts)
    {
        if (codeAsInts.Length != CodeLength)
            throw new ArgumentException(
                message: _formatErrorMessage,
                paramName: nameof(codeAsInts)
            );

        for (int i = 0; i < CodeLength; i++)
        {
            if (!RegistrationCodeHelper.IsInRange(_charIsAlpha[i], codeAsInts[i]))
                throw new ArgumentException(
                    message: _formatErrorMessage,
                    paramName: nameof(codeAsInts));
        }

        CodeAsInts = codeAsInts;
        CodeAsChars = CodeIntsToChars(CodeAsInts);
        Code = new(CodeAsChars);
    }

    public RegistrationCode(char[] codeAsChars)
    {
        if (codeAsChars.Length != CodeLength)
            throw new ArgumentException(
                message: _formatErrorMessage,
                paramName: nameof(codeAsChars)
            );

        for (int i = 0; i < CodeLength; i++)
        {
            if (!RegistrationCodeHelper.IsInRange(_charIsAlpha[i], (int)codeAsChars[i]))
                throw new ArgumentException(
                    message: _formatErrorMessage,
                    paramName: nameof(codeAsChars));
        }

        CodeAsChars = codeAsChars;
        CodeAsInts = CodeCharsToInts(CodeAsChars);
        Code = new(CodeAsChars);
    }

    public RegistrationCode(string code) : this(code.ToCharArray()) { }

    private static char[] CodeIntsToChars(int[] codeAsInts)
    {
        // Assumes that all values are in the proper range
        char[] codeAsChars = new char[CodeLength];
        for (int i = 0; i < CodeLength; i++)
            codeAsChars[i] = (char)codeAsInts[i];
        return codeAsChars;
    }

    private static int[] CodeCharsToInts(char[] codeAsChars)
    {
        // Assumes that all values are in the proper range
        int[] codeAsInts = new int[CodeLength];
        for (int i = 0; i < CodeLength; i++)
            codeAsInts[i] = (int)codeAsChars[i];
        return codeAsInts;
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
                = RegistrationCodeHelper.GenerateCharacter(_charIsAlpha[i], random);
        return new RegistrationCode(codeAsChars);
    }
}
