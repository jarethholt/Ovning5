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
    /// <remarks>
    /// A null value is used to represent a new, unpainted vehicle.
    /// Once painted, the value of Color should never again be null.
    /// This is why <see cref="PaintColor(string)"/> only takes non-
    /// null values.
    /// </remarks>
    public string? Color { get; private set; }

    /// <summary>
    /// Paint the vehicle.
    /// </summary>
    /// <param name="color">
    /// The color to paint the vehicle, or <c>null</c> to leave as-is.
    /// </param>
    public void PaintColor(string? color)
    {
        if (color is not null)
            Color = color;
    }
}
