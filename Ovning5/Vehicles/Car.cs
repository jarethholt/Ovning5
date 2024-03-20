namespace Ovning5.Vehicles;

public class Car : Vehicle
{
    public override int NumberOfWheels { get; } = 4;
    public string Make { get; }
    public string Model { get; }
    public int Year { get; }
    public int EngineCapacityInCC { get; }

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
