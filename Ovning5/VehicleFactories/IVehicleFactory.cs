using Ovning5.UI;
using Ovning5.Vehicles;

namespace Ovning5.VehicleFactories;

using ParameterSpec = IEnumerable<(string name, Type type)>;

internal interface IVehicleFactory
{
    public ParameterSpec Parameters { get; }
    public IVehicle CreateVehicle(string json);
    public IVehicle BuildVehicle(IUI ui);
    public static abstract IVehicle Example();
}
