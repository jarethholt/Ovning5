namespace Ovning5.Vehicles;

public class Airplane : Vehicle
{
    public override string Color { get; protected set; } = "White";
    public string Make { get; init; }
    public string Model { get; init; }
    public string AirplaneType { get; init; }
    public int PassengerCapacity { get; init; }

    public Airplane(
        int registrationNumber,
        string make,
        string model,
        string airplaneType,
        int passengerCapacity
    )
    {
        RegistrationNumber = registrationNumber;
        Make = make;
        Model = model;
        AirplaneType = airplaneType;
        PassengerCapacity = passengerCapacity;
    }

    public override string ToString() => $"{Color} {Make} {Model}";
}
