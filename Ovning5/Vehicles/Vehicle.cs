namespace Ovning5.Vehicles;

public abstract class Vehicle
{
    public int RegistrationNumber { get; protected set; }
    public virtual int NumberOfWheels { get; }
    public virtual string Color { get; protected set; } = "Unpainted";

    public void PaintColor(string color) => Color = color;

    public abstract override string ToString();
}
