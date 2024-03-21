using Ovning5.Registry;

namespace Ovning5.Vehicles;

public abstract record Vehicle(VehicleID VehicleID, string Color)
{
    public readonly VehicleID VehicleID = VehicleID;
    public virtual int NumberOfWheels { get; }
    public virtual string Color { get; init; } = Color;
}
