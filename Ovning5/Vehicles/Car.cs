namespace Ovning5.Vehicles;

public class Car : Vehicle
{
    /// <summary>
    /// Create a new Car.
    /// </summary>
    /// <param name="registrationNumber">
    /// Registration number of the car.
    /// </param>
    /// <param name="color">
    /// Color of the car, or <c>null</c> to leave unpainted.
    /// </param>
    public Car(int registrationNumber, string? color)
    {
        RegistrationNumber = registrationNumber;
        NumberOfWheels = 4;
        PaintColor(color);
    }
}
