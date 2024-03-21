using Ovning5.Registry;

namespace Ovning5.Vehicles;

public record Bus(
    VehicleID VehicleID,
    string Color,
    bool IsSingleCabin
) : Vehicle(VehicleID, Color)
{
    public override int NumberOfWheels
    {
        get => IsSingleCabin ? 6 : 10;
    }

    //public override string ToString()
    //{
    //    string cabins = IsSingleCabin ? "single-cabin" : "double-cabin";
    //    return $"{Color} {cabins} bus";
    //}
}
