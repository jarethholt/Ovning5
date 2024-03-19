namespace Ovning5.Registration;

public class Registry
{
    private readonly HashSet<RegistrationCode> _registrationCodes = [];

    public bool Contains(RegistrationCode registrationCode)
        => _registrationCodes.Contains(registrationCode);

    public bool Contains(string code)
    {
        try
        {
            RegistrationCode registrationCode = new(code);
            return Contains(registrationCode);
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    public bool Add(RegistrationCode registrationCode)
        => _registrationCodes.Add(registrationCode);

    public bool Remove(RegistrationCode registrationCode)
        => _registrationCodes.Remove(registrationCode);

    public RegistrationCode GenerateNewRegistrationCode(Random random)
    {
        RegistrationCode registrationCode;
        bool isNew;
        do
        {
            registrationCode = RegistrationCode.GenerateCode(random);
            isNew = Contains(registrationCode);
        } while (!isNew);
        return registrationCode;
    }
}
