namespace Ovning5.Vehicles;

public class Car : Vehicle
{
    public override int NumberOfWheels { get; init; } = 4;
    public string Make { get; init; }
    public string Model { get; init; }
    public int Year { get; init; }
    public int EngineCapacityInCC { get; init; }

    public Car(
        int registrationNumber,
        string make,
        string model,
        int year,
        int engineCapacityInCC
    )
    {
        RegistrationNumber = registrationNumber;
        Make = make;
        Model = model;
        Year = year;
        EngineCapacityInCC = engineCapacityInCC;
    }

    public override string ToString() => $"{Color} {Year} {Make} {Model} car";
}
