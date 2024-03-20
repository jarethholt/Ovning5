namespace Ovning5.Vehicles;

public class Airplane : Vehicle
{
    public override string Color { get; protected set; } = "White";
    public string Make { get; }
    public string Model { get; }
    public string AirplaneType { get; }
    public int PassengerCapacity { get; }

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
