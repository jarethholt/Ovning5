using Ovning5.Registry;

namespace Ovning5.Vehicles;

public record Car(
    VehicleID VehicleID,
    string Color,
    string Make,
    string Model,
    int Year,
    int EngineCapacityInCC
) : Vehicle(VehicleID, Color)
{
    public override int NumberOfWheels { get; } = 4;

    // public override string ToString() => $"{Color} {Year} {Make} {Model} car";
}
