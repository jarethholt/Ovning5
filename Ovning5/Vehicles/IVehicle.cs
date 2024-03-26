namespace Ovning5.Vehicles;

internal interface IVehicle
{
    // All vehicles have a VehicleID, color, and number of wheels
    VehicleID VehicleID { get; }
    string Color { get; }
    int NumberOfWheels { get; }

    // Vehicles are record class types, so they have automatic Equals and ToString
    bool Equals(object? obj);
    string ToString();

    // Each vehicle type also provides an example (default)
    static abstract Vehicle Example();
}
