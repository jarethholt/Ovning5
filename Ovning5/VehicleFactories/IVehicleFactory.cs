// Define how to store parameters for each Factory
global using ParameterSpec = (string name, System.Type type);
global using ParameterSpecs = System.Collections.Generic.List<
    (string name, System.Type type)>;

using Ovning5.UI;
using Ovning5.Vehicles;

namespace Ovning5.VehicleFactories;

internal interface IVehicleFactory
{
    public ParameterSpecs Parameters { get; }
    public Vehicle CreateVehicle(string json);
    public Vehicle BuildVehicle(IUI ui, IEnumerable<VehicleID> vehicleIDs);
    public static abstract Vehicle Example();
}
