using Ovning5.Registry;

namespace Ovning5.Vehicles;

internal record Airplane(
    VehicleID VehicleID,
    string Color,
    string Make,
    string Model,
    string AirplaneType,
    int PassengerCapacity
) : Vehicle(VehicleID, Color)
{
    //public override string ToString() => $"{Color} {Make} {Model}";
}
