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

    public IVehicleFactory ChooseVehicleFactory(IUI ui)
    {
        ui.WriteLine($"The following vehicle types are available: {string.Join(", ", _vehicleFactories.Keys)}");
        return Utilities.AskForDictValue("Please choose a vehicle: ", _vehicleFactories, ui);
    }
}
