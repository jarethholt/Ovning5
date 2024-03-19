namespace Ovning5.Vehicles;

public class Motorcycle : Vehicle
{
    public override int NumberOfWheels { get; init; } = 2;
    public string Make { get; init; }
    public string Model { get; init; }
    public int Year { get; init; }
    public string MotorcycleType { get; init; }

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
