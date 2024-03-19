namespace Ovning5.Vehicles;

/// <summary>
/// Abstract base class for all vehicles.
/// </summary>
public abstract class Vehicle
{
    /// <summary>
    /// The registration number for the vehicle.
    /// </summary>
    /// <remarks>
    /// Registration numbers for vehicles do not change. Thus this
    /// value must be supplied at initialization.
    /// </remarks>
    public int RegistrationNumber { get; init; }
    /// <summary>
    /// The number of wheels on the vehicle.
    /// </summary>
    /// <remarks>Often a constant of the vehicle type.</remarks>
    public virtual int NumberOfWheels { get; init; }
    /// <summary>
    /// The color of the vehicle.
    /// </summary>
    public string Color { get; private set; } = "Unpainted";

    /// <summary>
    /// Paint the vehicle.
    /// </summary>
    /// <param name="color">The color to paint the vehicle.</param>
    public void PaintColor(string color) => Color = color;

    /// <summary>
    /// Abstract requiring a custom override of ToString for subclasses.
    /// </summary>
    /// <returns>A string description.</returns>
    public abstract override string ToString();
}
