using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ovning5.Registration;

public class Registry
{
    public HashSet<RegistrationCode> RegistrationCodes { get; init; }

    public Registry() => RegistrationCodes = [];

    public Registry(IEnumerable<RegistrationCode> registrationCodes)
        => RegistrationCodes = new(registrationCodes);

    public bool Contains(RegistrationCode registrationCode)
        => RegistrationCodes.Contains(registrationCode);

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
        => RegistrationCodes.Add(registrationCode);

    public bool Remove(RegistrationCode registrationCode)
        => RegistrationCodes.Remove(registrationCode);

    public RegistrationCode GenerateAndAddNewCode(Random random)
    {
        RegistrationCode registrationCode;
        bool isNew;
        do
        {
            registrationCode = RegistrationCode.GenerateCode(random);
            isNew = Contains(registrationCode);
        } while (!isNew);

        RegistrationCodes.Add(registrationCode);
        return registrationCode;
    }

    public string Serialize() => JsonSerializer.Serialize(RegistrationCodes.ToList());

    public static Registry Deserialize(string jsonString)
    {
        var codes = JsonSerializer.Deserialize<List<RegistrationCode>>(jsonString);
        return codes is null ? new Registry() : new Registry(codes);
    }
}
