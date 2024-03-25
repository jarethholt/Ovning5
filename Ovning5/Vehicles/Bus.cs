namespace Ovning5.Vehicles;

internal record Bus(
    VehicleID VehicleID,
    string Color,
    bool IsSingleCabin
) : Vehicle(VehicleID, Color), IVehicle
{
    public override int NumberOfWheels
    {
        get => IsSingleCabin ? 6 : 10;
    }

    public static new IVehicle Example()
        => new Bus(new VehicleID("GHI789"), "Red", true);

    //public override string ToString()
    //{
    //    string cabins = IsSingleCabin ? "single-cabin" : "double-cabin";
    //    return $"{Color} {cabins} bus";
    //}
}
