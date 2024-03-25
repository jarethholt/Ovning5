namespace Ovning5.Vehicles;

internal record Motorcycle(
    VehicleID VehicleID,
    string Color,
    string Make,
    string Model,
    int Year,
    string MotorcycleType
) : Vehicle(VehicleID, Color), IVehicle
{
    public override int NumberOfWheels { get; } = 2;

    public static new IVehicle Example()
        => new Motorcycle(new VehicleID("DEF456"), "Black", "Kawasaki", "Ninja ZX", 2024, "Sportbike");

    //public override string ToString() => $"{Color} {Year} {Make} {Model}";
}
