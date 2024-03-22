namespace Ovning5.Vehicles;

internal abstract record Vehicle(VehicleID VehicleID, string Color) : IVehicle
{
    public VehicleID VehicleID { get; } = VehicleID;
    public virtual int NumberOfWheels { get; }
    public virtual string Color { get; init; } = Color;
}
