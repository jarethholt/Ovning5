using Ovning5.Vehicles;
using System.Text.Json;

namespace Ovning5.VehicleFactories;

internal class BoatFactory : VehicleFactory, IVehicleFactory
{
    public override string VehicleTypeName => "Boat";
    public override ParameterSpecs Parameters =>
    [
        ("VehicleID", typeof(VehicleID)),
        ("Color", typeof(string)),
        ("LengthInMeters", typeof(int)),
        ("Name", typeof(string))
    ];

    public override Vehicle CreateVehicle(string json)
        => JsonSerializer.Deserialize<Boat>(json)
        ?? throw new ArgumentException(
            $"Could not deserialize this JSON as type {VehicleTypeName}: {json}");

    public static new Vehicle Example() => Boat.Example();
}
