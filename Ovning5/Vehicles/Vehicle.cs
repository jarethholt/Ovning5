namespace Ovning5.Vehicles;

public abstract class Vehicle
{
    public int RegistrationNumber { get; init; }
    public virtual int NumberOfWheels { get; init; }
    public virtual string Color { get; protected set; } = "Unpainted";

    public void PaintColor(string color) => Color = color;

    public abstract override string ToString();
}
