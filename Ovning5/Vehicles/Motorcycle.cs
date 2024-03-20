namespace Ovning5.Vehicles;

public class Motorcycle : Vehicle
{
    public override int NumberOfWheels { get; } = 2;
    public string Make { get; }
    public string Model { get; }
    public int Year { get; }
    public string MotorcycleType { get; }

    public Motorcycle(
        int registrationNumber,
        string make,
        string model,
        int year,
        string motorcycleType
    )
    {
        RegistrationNumber = registrationNumber;
        Make = make;
        Model = model;
        Year = year;
        MotorcycleType = motorcycleType;
    }

    public override string ToString() => $"{Color} {Year} {Make} {Model}";
}
