using Ovning5.VehicleRegistry;

namespace Ovning5.Vehicles;

public abstract record class Vehicle
{
    private readonly VehicleRegistry<Vehicle> _registry = null!;
    private readonly VehicleID _vehicleID;
    public VehicleID VehicleID
    {
        get => _vehicleID;
        init => _vehicleID = _registry.GenerateNewID(new Random());
    }
    public virtual int NumberOfWheels { get; }
    public virtual string Color { get; } = "Unpainted";
}
