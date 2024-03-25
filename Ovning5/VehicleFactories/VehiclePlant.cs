using Ovning5.UI;
using Ovning5.Vehicles;

namespace Ovning5.VehicleFactories;

internal class VehiclePlant
{
    private readonly Dictionary<string, IVehicleFactory> _vehicleFactories = new()
    {
        { "Airplane", new AirplaneFactory() },
        { "Boat", new BoatFactory() },
        { "Bus", new BusFactory() },
        { "Car", new CarFactory() },
        { "Motorcycle", new MotorcycleFactory() },
    };

    public IVehicle BuildVehicle(string vehicleTypeName, IUI ui)
    {
        if (!_vehicleFactories.TryGetValue(vehicleTypeName, out var factory))
            throw new ArgumentException(
                $"Cannot get a VehicleFactory for type {vehicleTypeName}; "
                + $"available vehicles are {string.Join(", ", _vehicleFactories.Keys)}");
        return factory.BuildVehicle(ui);
    }
}
