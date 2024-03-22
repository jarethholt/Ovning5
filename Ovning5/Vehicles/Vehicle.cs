using Ovning5.Registry;

namespace Ovning5.Vehicles;

internal abstract record Vehicle(VehicleID VehicleID, string Color) : IVehicle
{
    public readonly VehicleID VehicleID = VehicleID;
    public virtual int NumberOfWheels { get; }
    public virtual string Color { get; init; } = Color;
}
