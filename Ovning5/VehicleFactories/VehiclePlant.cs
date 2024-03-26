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

    public string[] AvailableVehicles
    {
        get => [.. _vehicleFactories.Keys];
    }

    // Choose the right factory for the vehicle that needs to be made
    public string ChooseVehicleType(IUI ui)
    {
        ui.WriteLine(
            "The following vehicle types are available: "
            + $"{string.Join(", ", AvailableVehicles)}");
        return Utilities.AskForDictKey(
            "Please choose a vehicle: ", _vehicleFactories, ui);
    }

    public IVehicleFactory ChooseVehicleFactory(IUI ui)
    {
        ui.WriteLine(
            "The following vehicle types are available: "
            + $"{string.Join(", ", AvailableVehicles)}");
        return Utilities.AskForDictValue(
            "Please choose a vehicle: ", _vehicleFactories, ui);
    }

    // Get all possible vehicle parameters, and which vehicles they apply to
    public Dictionary<string, (Type type, List<string> appliesTo)> GetAllParameters()
    {
        Dictionary<string, (Type type, List<string> appliesTo)> allParams = [];
        foreach (var kvp in _vehicleFactories)
        {
            string vehicleTypeName = kvp.Key;
            VehicleFactory factory = kvp.Value;
            foreach ((string name, Type type) in factory.Parameters)
            {
                if (!allParams.ContainsKey(name))
                    allParams.Add(name, (type, []));
                allParams[name].appliesTo.Add(vehicleTypeName);
            }
        }
        return allParams;
    }
}
