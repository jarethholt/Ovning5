using Ovning5.Vehicles;
using System.Text.Json;

namespace Ovning5.VehicleFactories;

internal class MotorcycleFactory : VehicleFactory, IVehicleFactory
{
    protected override string VehicleTypeName => "Motorcycle";
    protected override (string name, Type type)[] Parameters =>
    [
        ("VehicleID", typeof(VehicleID)),
        ("Color", typeof(string)),
        ("Make", typeof(string)),
        ("Model", typeof(string)),
        ("Year", typeof(int)),
        ("MotorcycleType", typeof(string))
    ];

    public override Vehicle CreateVehicle(string json)
        => JsonSerializer.Deserialize<Motorcycle>(json)
        ?? throw new ArgumentException(
            $"Could not deserialize this JSON as type {VehicleTypeName}: {json}");

    public static new Vehicle Example() => Motorcycle.Example();
}
