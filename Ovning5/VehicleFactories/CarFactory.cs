using Ovning5.Vehicles;
using System.Text.Json;

namespace Ovning5.VehicleFactories;

internal class CarFactory : VehicleFactory, IVehicleFactory
{
    public override string VehicleTypeName => "Car";
    public override ParameterSpecs Parameters =>
    [
        ("VehicleID", typeof(VehicleID)),
        ("Color", typeof(string)),
        ("Make", typeof(string)),
        ("Model", typeof(string)),
        ("Year", typeof(int)),
        ("EngineCapacityInCC", typeof(int))
    ];

    public override Vehicle CreateVehicle(string json)
        => JsonSerializer.Deserialize<Car>(json)
        ?? throw new ArgumentException(
            $"Could not deserialize this JSON as type {VehicleTypeName}: {json}");

    public static new Vehicle Example() => Car.Example();
}
