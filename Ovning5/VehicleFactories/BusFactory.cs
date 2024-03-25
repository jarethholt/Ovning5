using Ovning5.Vehicles;

namespace Ovning5.VehicleFactories;

internal class BusFactory : VehicleFactory<Bus>
{
    protected override string VehicleTypeName => "Bus";
    protected override (string name, Type type)[] Parameters =>
    [
        ("VehicleID", typeof(VehicleID)),
        ("Color", typeof(string)),
        ("IsSingleCabin", typeof(bool))
    ];
}
