namespace Ovning5.Vehicles;

/* Abstract class for Vehicle.
 * During most of development, this class felt very redundant with the
 * IVehicle interface. However, when the static method Example was added,
 * I could no longer use IVehicle as a generic since the IVehicle static
 * method was "not the most resolved method" (or similar message). In
 * those cases, using Vehicle as the generic worked.
 */
internal abstract record Vehicle(VehicleID VehicleID, string Color) : IVehicle
{
    public VehicleID VehicleID { get; } = VehicleID;
    // NumberOfWheels is set differently for each subclass
    public virtual int NumberOfWheels { get; }
    // In earlier implementations, some classes had default colors,
    // which could be handled by Color being virtual and having init
    public string Color { get; } = Color;

    // I wish I could make this an abstract or at least virtual method :/
    public static Vehicle Example() => throw new NotImplementedException();
}
