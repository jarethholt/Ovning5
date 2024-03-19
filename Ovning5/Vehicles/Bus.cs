namespace Ovning5.Vehicles;

public class Bus : Vehicle
{
    public bool IsSingleCabin { get; init; }

    public override int NumberOfWheels
    {
        get => IsSingleCabin ? 6 : 10;
    }

    /// <summary>
    /// Create a new Bus.
    /// </summary>
    /// <param name="registrationNumber">
    /// Registration number of the bus.
    /// </param>
    /// <param name="isSingleCabin">
    /// Whether the bus is single- or double-cabin. Single-cabin buses
    /// have 6 wheels; double-cabin buses have 10.
    /// </param>
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
