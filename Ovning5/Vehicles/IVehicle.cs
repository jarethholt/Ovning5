namespace Ovning5.Vehicles;

public interface IVehicle
{
    VehicleID VehicleID { get; }
    string Color { get; init; }
    int NumberOfWheels { get; }

    bool Equals(object? obj);
    string ToString();
}
