using Ovning5.Vehicles;

namespace Ovning5.VehicleFactories;

internal class AirplaneFactory : VehicleFactory<Airplane>
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
}
