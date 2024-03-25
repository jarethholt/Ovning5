namespace Ovning5.Vehicles;

internal record Boat(
    VehicleID VehicleID,
    string Color,
    int LengthInMeters,
    string Name
) : Vehicle(VehicleID, Color), IVehicle
{
    private const float _metersToFeet = 3.28084f;
    public override int NumberOfWheels { get; } = 0;
    public int LengthInFeet => (int)(LengthInMeters * _metersToFeet);

    public static new Vehicle Example()
        => new Boat(new VehicleID("JKL012"), "White", 25, "Calculon's Pride");

    //public override string ToString() => $"{LengthInMeters}-m {Color} boat: {Name}";
}
