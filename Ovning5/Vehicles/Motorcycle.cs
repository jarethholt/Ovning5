namespace Ovning5.Vehicles;

public class Motorcycle : Vehicle
{
    /// <summary>
    /// Create a new Motorcycle.
    /// </summary>
    /// <param name="registrationNumber">
    /// The registration number of the motorcycle.
    /// </param>
    /// <param name="color">
    /// Color of the motorcycle, or <c>null</c> to leave unpainted.
    /// </param>
    public Motorcycle(int registrationNumber, string? color)
    {
        RegistrationNumber = registrationNumber;
        NumberOfWheels = 2;
        PaintColor(color);
    }
}
