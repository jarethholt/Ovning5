namespace Ovning5.Vehicles;

public class Car : Vehicle
{
    public override int NumberOfWheels { get; init; } = 4;
    /// <summary>
    /// The make of the car, i.e. the manufacturer, e.g. "Toyota".
    /// </summary>
    public string Make { get; init; }
    /// <summary>
    /// The model of the car, e.g. "Civic", "Altima".
    /// </summary>
    public string Model { get; init; }
    /// <summary>
    /// The year the car was manufactured.
    /// </summary>
    public int Year { get; init; }

    /// <summary>
    /// Create a new Car.
    /// </summary>
    /// <param name="registrationNumber">
    /// Registration number of the car.
    /// </param>
    /// <param name="make">The make of the car.</param>
    /// <param name="model">The model of the car.</param>
    /// <param name="year">The year the car was manufactured.</param>
    public Car(
        int registrationNumber,
        string make,
        string model,
        int year
    )
    {
        RegistrationNumber = registrationNumber;
        Make = make;
        Model = model;
        Year = year;
    }

    public override string ToString() => $"{Color} {Year} {Make} {Model} car";
}
