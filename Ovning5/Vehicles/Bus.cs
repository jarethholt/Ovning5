namespace Ovning5.Vehicles;

public class Bus : Vehicle
{
    public bool IsSingleCabin { get; init; }
    public override int NumberOfWheels
    {
        get => IsSingleCabin ? 6 : 10;
    }

    public Bus(int registrationNumber, bool isSingleCabin)
    {
        RegistrationNumber = registrationNumber;
        IsSingleCabin = isSingleCabin;
    }

    public override string ToString()
    {
        string cabins = IsSingleCabin ? "single-cabin" : "double-cabin";
        return $"{Color} {cabins} bus";
    }
}
