using Ovning5.UI;
using Ovning5.Vehicles;

namespace Ovning5.VehicleFactories;

// Helper class: Ask for necessary parameters and construct the JSON string
internal static class VehicleFactoryHelper
{
    public static List<(string name, string value)> AskForParameters(
        string vehicleTypeName,
        ParameterSpecs parameters,
        IUI ui,
        IEnumerable<VehicleID>? vehicleIDs = null)
    {
        ui.WriteLine($"Please provide the necessary parameters for a {vehicleTypeName}.");
        List<(string name, string value)> args = [];
        foreach ((string name, Type type) in parameters)
        {
            /* The only types used are VehicleID, string, int, and bool.
             * This is another place where reflection could help make this
             * process less error-prone, but I would need to implement an
             * `Utilities.AskFor*` function for each type regardless.
             * 
             * Also, I couldn't figure out how to put a switch statement on
             * a System.Type instance.
             * 
             * FYI: The string values (including VehicleID) need to be wrapped
             * in quotes.
             */
            if (type.Equals(typeof(VehicleID)))
            {
                // For VehicleID, use an empty input to generate new random
                string prompt = 
                    $"{name} (string with format {VehicleID.CodeFormat}; "
                    + "blank for new random value): ";
                VehicleID? result = Utilities.AskForVehicleID(
                    prompt, ui, isEmptyOk: true);
                VehicleID value;
                if (result is null)
                {
                    Random random = new();
                    if (vehicleIDs is null)
                        value = VehicleID.GenerateID(random);
                    else
                        value = VehicleID.GenerateUniqueID(random, vehicleIDs);
                }
                else
                    value = (VehicleID)result;
                args.Add((name, $"\"{value}\""));
            }
            else if (type.Equals(typeof(bool)))
            {
                string prompt = $"{name} ([y]es or [n]o): ";
                bool value = Utilities.AskForYesNo(prompt, ui);
                // JSON doesn't play well with bool; convert to integer
                int intValue = value ? 1 : 0;
                args.Add((name, intValue.ToString()));
            }
            else if (type.Equals(typeof(int)))
            {
                string prompt = $"{name} (positive int): ";
                int value = (int)Utilities.AskForPositiveInt(prompt, ui)!;
                args.Add((name, value.ToString()));
            }
            else if (type.Equals(typeof(string)))
            {
                string prompt = $"{name} (string): ";
                string value = (string)Utilities.AskForString(prompt, ui)!;
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

    // Create a string from the parameter values for JSON deserialization
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
    public abstract string VehicleTypeName { get; }
    public abstract ParameterSpecs Parameters { get; }

    public List<(string name, string value)> AskForParameters(
        IUI ui,
        IEnumerable<VehicleID>? vehicleIDs = null)
        => VehicleFactoryHelper.AskForParameters(
            VehicleTypeName, Parameters, ui, vehicleIDs);

    public abstract Vehicle CreateVehicle(string json);

    public Vehicle BuildVehicle(IUI ui, IEnumerable<VehicleID>? vehicleIDs = null)
    {
        var parameterValues = AskForParameters(ui, vehicleIDs);
        string json = VehicleFactoryHelper.CreateJsonString(parameterValues);
        return CreateVehicle(json);
    }

    public static Vehicle Example() => throw new NotImplementedException();
}
