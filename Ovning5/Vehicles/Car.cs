namespace Ovning5.Vehicles;

internal record Car(
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
