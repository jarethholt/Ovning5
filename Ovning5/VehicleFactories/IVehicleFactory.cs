using Ovning5.UI;
using Ovning5.Vehicles;

namespace Ovning5.VehicleFactories;

using ParameterSpec = IEnumerable<(string name, Type type)>;

internal interface IVehicleFactory
{
    public ParameterSpec Parameters { get; }
    public Vehicle CreateVehicle(string json);
    public Vehicle BuildVehicle(IUI ui);
    public static abstract Vehicle Example();
}
