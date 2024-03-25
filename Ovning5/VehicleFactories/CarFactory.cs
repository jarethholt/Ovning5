using Ovning5.Vehicles;

namespace Ovning5.VehicleFactories;

internal class CarFactory : VehicleFactory<Car>
{
    protected override string VehicleTypeName => "Car";
    protected override (string name, Type type)[] Parameters =>
    [
        ("VehicleID", typeof(VehicleID)),
        ("Color", typeof(string)),
        ("Make", typeof(string)),
        ("Model", typeof(string)),
        ("Year", typeof(int)),
        ("EngineCapacityInCC", typeof(int))
    ];
}
