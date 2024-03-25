using System.Text.Json;

namespace Ovning5.Vehicles;

internal interface IVehicle
{
    VehicleID VehicleID { get; }
    string Color { get; init; }
    int NumberOfWheels { get; }

    bool Equals(object? obj);
    string ToString();
    static abstract Vehicle Example();
}
