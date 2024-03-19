namespace Ovning5.Vehicles;

public class Bus : Vehicle
{
    /// <summary>
    /// Create a new Bus.
    /// </summary>
    /// <param name="registrationNumber">
    /// Registration number of the bus.
    /// </param>
    /// <param name="numberOfWheels">
    /// Number of wheels on the bus; usually 6 for a standard (single-
    /// cabin) bus and 10 for a long (double-cabin).
    /// </param>
    /// <param name="color">
    /// Color of the bus, or <c>null</c> to leave unpainted.
    /// </param>
    public Bus(int registrationNumber, int numberOfWheels, string? color)
    {
        RegistrationNumber = registrationNumber;
        NumberOfWheels = numberOfWheels;
        PaintColor(color);
    }
}
