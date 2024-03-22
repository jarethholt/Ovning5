using Ovning5.Registry;

namespace Ovning5.Vehicles;

internal interface IVehicle
{
    string Color { get; init; }
    int NumberOfWheels { get; }

    bool Equals(object? obj);
    string ToString();
}
