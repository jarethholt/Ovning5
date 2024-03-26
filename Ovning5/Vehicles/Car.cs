namespace Ovning5.Vehicles;

internal record Car(
    VehicleID VehicleID,
    string Color,
    string Make,
    string Model,
    int Year,
    int EngineCapacityInCC
) : Vehicle(VehicleID, Color), IVehicle
{
    public override int NumberOfWheels { get; } = 4;

    public static new Vehicle Example()
        => new Car(
            new VehicleID("ABC123"),
            "Beige",
            "Toyota",
            "Corolla",
            2002,
            1000);

    // public override string ToString() => $"{Color} {Year} {Make} {Model} car";
}
