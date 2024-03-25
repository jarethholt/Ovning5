using Ovning5.Vehicles;

namespace Ovning5.VehicleFactories;

internal class BoatFactory : VehicleFactory<Boat>
{
    protected override string VehicleTypeName => "Boat";
    protected override (string name, Type type)[] Parameters =>
    [
        ("VehicleID", typeof(VehicleID)),
        ("Color", typeof(string)),
        ("LengthInMeters", typeof(int)),
        ("Name", typeof(string))
    ];
}
