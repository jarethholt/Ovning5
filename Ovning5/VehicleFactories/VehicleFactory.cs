using Ovning5.UI;
using Ovning5.Vehicles;
using System.Text.Json;

namespace Ovning5.VehicleFactories;

using ParameterSpec = IEnumerable<(string name, Type type)>;

internal static class VehicleFactoryHelper
{
    public static List<(string name, string value)> AskForParameters(
        string vehicleTypeName,
        IEnumerable<(string name, Type type)> parameters,
        IUI ui)
    {
        ui.WriteLine($"Please provide the necessary parameters for a {vehicleTypeName}.");
        List<(string name, string value)> args = [];
        foreach ((string name, Type type) in parameters)
        {
            if (type.Equals(typeof(VehicleID)))
            {
                string prompt = $"{name} (string with format {VehicleID.CodeFormat}): ";
                VehicleID value = Utilities.AskForVehicleID(prompt, ui);
                args.Add((name, $"\"{value}\""));
            }
            else if (type.Equals(typeof(bool)))
            {
                string prompt = $"{name} ([y]es or [n]o): ";
                bool value = Utilities.AskForYesNo(prompt, ui);
                args.Add((name, value.ToString()));
            }
            else if (type.Equals(typeof(int)))
            {
                string prompt = $"{name} (positive int): ";
                int value = Utilities.AskForPositiveInt(prompt, ui);
                args.Add((name, value.ToString()));
            }
            else if (type.Equals(typeof(string)))
            {
                string prompt = $"{name} (string): ";
                string value = Utilities.AskForString(prompt, ui);
                args.Add((name, $"\"{value}\""));
            }
            else
            {
                throw new ArgumentException(
                    $"Parameter {name} has a type {type.Name} that was unhandled. "
                    + "Expected one of: VehicleID, bool, int, or string");
            }
        }
        return args;
    }

    public static string CreateJsonString(
        IEnumerable<(string name, string value)> args)
    {
        string[] argGroups = new string[args.Count()];
        for (int i = 0; i < args.Count(); i++)
        {
            (string name, string value) = args.ElementAt(i);
            argGroups[i] = $"\"{name}\":{value}";
        }
        return "{" + string.Join(',', argGroups) + "}";
    }
}

internal abstract class VehicleFactory : IVehicleFactory
{
    protected abstract string VehicleTypeName { get; }
    protected abstract ParameterSpec Parameters { get; }

    ParameterSpec IVehicleFactory.Parameters => Parameters;

    public List<(string name, string value)> AskForParameters(IUI ui)
        => VehicleFactoryHelper.AskForParameters(VehicleTypeName, Parameters, ui);

    public abstract IVehicle CreateVehicle(string json);

    public IVehicle BuildVehicle(IUI ui)
    {
        var parameterValues = AskForParameters(ui);
        string json = VehicleFactoryHelper.CreateJsonString(parameterValues);
        return CreateVehicle(json);
    }

    public static IVehicle Example() => throw new NotImplementedException();
}
