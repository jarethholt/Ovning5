using Ovning5.Vehicles;
using System.Text.Json;

namespace Ovning5.VehicleFactories;

internal class AirplaneFactory : VehicleFactory, IVehicleFactory
{
    public override string VehicleTypeName => "Airplane";
    public override ParameterSpecs Parameters =>
    [
        ("VehicleID", typeof(VehicleID)),
        ("Color", typeof(string)),
        ("Make", typeof(string)),
        ("Model", typeof(string)),
        ("AirplaneType", typeof(string)),
        ("PassengerCapacity", typeof(int))
    ];

    public override Vehicle CreateVehicle(string json)
        => JsonSerializer.Deserialize<Airplane>(json)
        ?? throw new ArgumentException(
            $"Could not deserialize this JSON as type {VehicleTypeName}: {json}");

    public static new Vehicle Example() => Airplane.Example();
}
