using Ovning5.Vehicles;
using System.Text.Json;

namespace Ovning5.VehicleFactories;

internal class BusFactory : VehicleFactory, IVehicleFactory
{
    protected override string VehicleTypeName => "Bus";
    protected override (string name, Type type)[] Parameters =>
    [
        ("VehicleID", typeof(VehicleID)),
        ("Color", typeof(string)),
        ("IsSingleCabin", typeof(bool))
    ];

    public override Vehicle CreateVehicle(string json)
        => JsonSerializer.Deserialize<Bus>(json)
        ?? throw new ArgumentException(
            $"Could not deserialize this JSON as type {VehicleTypeName}: {json}");

    public static new Vehicle Example() => Bus.Example();
}
