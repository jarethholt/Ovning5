using Ovning5.Vehicles;
using System.Text.Json;

namespace Ovning5.VehicleFactories;

internal class AirplaneFactory : VehicleFactory, IVehicleFactory
{
    protected override string VehicleTypeName => "Airplane";
    protected override (string name, Type type)[] Parameters =>
    [
        ("VehicleID", typeof(VehicleID)),
        ("Color", typeof(string)),
        ("Make", typeof(string)),
        ("Model", typeof(string)),
        ("AirplaneType", typeof(string)),
        ("PassengerCapacity", typeof(int))
    ];

    public override IVehicle CreateVehicle(string json)
        => JsonSerializer.Deserialize<Airplane>(json)
        ?? throw new ArgumentException(
            $"Could not deserialize this JSON as type {VehicleTypeName}: {json}");

    public static new IVehicle Example() => Airplane.Example();
}
