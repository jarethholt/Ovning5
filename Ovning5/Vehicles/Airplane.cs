namespace Ovning5.Vehicles;

internal record Airplane(
    VehicleID VehicleID,
    string Color,
    string Make,
    string Model,
    string AirplaneType,
    int PassengerCapacity
) : Vehicle(VehicleID, Color), IVehicle
{
    public static new IVehicle Example()
        => new Airplane(new VehicleID("MNO345"), "White", "Boeing", "747", "Passenger jet", 350);

    //public override string ToString() => $"{Color} {Make} {Model}";
}
