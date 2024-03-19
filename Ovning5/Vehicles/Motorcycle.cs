namespace Ovning5.Vehicles;

public class Motorcycle : Vehicle
{
    public string Make { get; init; }
    public string Model { get; init; }
    public int Year { get; init; }
    public string MotorcycleType { get; init; }

    /// <summary>
    /// Create a new Motorcycle.
    /// </summary>
    /// <param name="registrationNumber">
    /// The registration number of the motorcycle.
    /// </param>
    public Motorcycle(
        int registrationNumber,
        string make,
        string model,
        int year,
        string motorcycleType
    )
    {
        RegistrationNumber = registrationNumber;
        NumberOfWheels = 2;
        Make = make;
        Model = model;
        Year = year;
        MotorcycleType = motorcycleType;
    }

    public override string ToString() => $"{Color} {Year} {Make} {Model}";
}
