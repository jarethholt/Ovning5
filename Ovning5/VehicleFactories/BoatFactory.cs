using Ovning5.Vehicles;
using System.Text.Json;

namespace Ovning5.VehicleFactories;

internal class BoatFactory : VehicleFactory, IVehicleFactory
{
    protected override string VehicleTypeName => "Boat";
    protected override (string name, Type type)[] Parameters =>
    [
        ("VehicleID", typeof(VehicleID)),
        ("Color", typeof(string)),
        ("LengthInMeters", typeof(int)),
        ("Name", typeof(string))
    ];

    public override IVehicle CreateVehicle(string json)
        => JsonSerializer.Deserialize<Boat>(json)
        ?? throw new ArgumentException(
            $"Could not deserialize this JSON as type {VehicleTypeName}: {json}");

    public static new IVehicle Example() => Boat.Example();
}
