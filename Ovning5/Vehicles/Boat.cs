using Ovning5.Registry;

namespace Ovning5.Vehicles;

public record Boat(
    VehicleID VehicleID,
    string Color,
    int LengthInMeters,
    string Name
) : Vehicle(VehicleID, Color)
{
    private const float _metersToFeet = 3.28084f;
    public override int NumberOfWheels { get; } = 0;
    public int LengthInFeet => (int)(LengthInMeters * _metersToFeet);

    //public override string ToString() => $"{LengthInMeters}-m {Color} boat: {Name}";
}
