using Ovning5.UI;

namespace Ovning5.VehicleFactories;

internal class VehiclePlant
{
    // Keep an instance of each VehicleFactory ready to create vehicles
    private readonly Dictionary<string, VehicleFactory> _vehicleFactories = new()
    {
        { "Airplane", new AirplaneFactory() },
        { "Boat", new BoatFactory() },
        { "Bus", new BusFactory() },
        { "Car", new CarFactory() },
        { "Motorcycle", new MotorcycleFactory() },
    };

    // Choose the right factory for the vehicle that needs to be made
    public IVehicleFactory ChooseVehicleFactory(IUI ui)
    {
        ui.WriteLine(
            "The following vehicle types are available: "
            + $"{string.Join(", ", _vehicleFactories.Keys)}");
        return Utilities.AskForDictValue(
            "Please choose a vehicle: ",
            _vehicleFactories,
            ui);
    }
}
