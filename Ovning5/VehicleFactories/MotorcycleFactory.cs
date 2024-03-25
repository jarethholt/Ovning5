using Ovning5.Vehicles;

namespace Ovning5.VehicleFactories;

internal class MotorcycleFactory : VehicleFactory<Motorcycle>
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
}
