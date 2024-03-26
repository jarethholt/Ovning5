using Ovning5.UI;
using System.Reflection;
using System.Text.Json;

namespace Ovning5.Vehicles;

/* Original attempt to implement a VehicleBuilder (now VehicleFactory)
 * My goal here was to use reflection to automate the construction of
 * concrete VehicleFactory types. I found I could do the following:
 * - Find the current module/assembly;
 * - Use it to find all the concrete _vehicleTypes and _vehicleTypeNames;
 * - Find the names and types for all parameters in a vehicle's constructor
 *   (which differ from vehicle to vehicle); and
 * - Convert these arguments into a readable JSON string for serialization.
 * 
 * Where this approach failed/ran out of time is that I have to provide a
 * generic type T to JsonSerializer.Deserialize to get the correct object
 * out. However, I cannot pass in an actual System.Type instance as T. I
 * found some resources online for how to get around this, but they looked
 * too complicated to implement within the deadline.
 * 
 * In the end, I opted for an explicit approach, providing the vehicle name
 * and parameters to each *Factory class. This would get more and more annoying
 * and error-prone as more classes are added. On the other hand, using
 * reflection for such a core part of the code seems like it would be slow
 * and potentially break type safety and encapsulation.
 */
internal static class VehicleBuilder_orig
{
    private static readonly Module _currModule = typeof(VehicleBuilder_orig).Module;
    private static readonly Type[] _vehicleTypes
        = _currModule.FindTypes(ConcreteVehicleFilter, null);
    private static readonly string[] _vehicleTypeNames
        = _vehicleTypes.Select(type => type.Name).ToArray();

    public static string[] AvailableVehicles { get => _vehicleTypeNames; }

    public static Type AskForVehicleType(IUI ui)
    {
        ui.WriteLine("These are the available vehicle types to build:");
        for (int i = 0; i < AvailableVehicles.Length; i++)
        {
            ui.WriteLine($"{i+1}: {AvailableVehicles[i]}");
        }
        string prompt = $"Please choose a vehicle 1-{AvailableVehicles.Length}: ";
        int index = Utilities.AskForInt(prompt, 1, AvailableVehicles.Length, ui);
        return _vehicleTypes[index];
    }

    // Ask for values for all constructor parameters
    public static List<(string name, string value)> AskForConstructorParameters(
        Type vehicleType,
        IUI ui)
    {
        ui.WriteLine("Please provide the necessary parameters for the vehicle.");
        ParameterSpecs parameters = GetConstructorParameters(vehicleType);
        List<(string name, string value)> args = [];
        foreach ((string name, Type type) in parameters)
        {
            if (type.Equals(typeof(VehicleID)))
            {
                string prompt = $"{name} (string with format {VehicleID.CodeFormat}): ";
                VehicleID? result = Utilities.AskForVehicleID(
                    prompt, ui, isEmptyOk: true);
                VehicleID value;
                if (result is null)
                    value = VehicleID.GenerateID(new Random());
                else
                    value = (VehicleID)result;
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

    // Get the Vehicle System.Type from a string name
    public static Type GetVehicleType(string vehicleTypeName)
    {
        return _vehicleTypes.FirstOrDefault(type => type.Name == vehicleTypeName)
               ?? throw new ArgumentOutOfRangeException(
                   nameof(vehicleTypeName),
                   $"Could not find {vehicleTypeName} among the known vehicle types: "
                   + $"{string.Join(", ", AvailableVehicles)}");
    }

    // Get all the parameters of a constructor
    public static ParameterSpecs GetConstructorParameters(Type vehicleType)
    {
        var constructors = vehicleType.GetConstructors();
        if (constructors.Length != 1)
            throw new Exception(
                $"Expected 1 constructor for {vehicleType.Name}, "
                + $"got {constructors.Length}");
        var constructor = constructors[0];
        var parameters = constructor.GetParameters();
        return parameters.Select(param => (param.Name!, param.ParameterType)).ToList();
    }

    private static string CreateJsonString(IEnumerable<(string name, string value)> args)
    {
        string[] argGroups = new string[args.Count()];
        for (int i = 0; i < args.Count(); i++)
        {
            (string name, string value) = args.ElementAt(i);
            argGroups[i] = $"\"{name}\":{value}";
        }
        return "{" + string.Join(',', argGroups) + "}";
    }

    // Can serialize and deserialize given the generic T
    public static T ConstructFromArgs<T>(
        IEnumerable<(string name, string value)> args) where T : class, IVehicle
    {
        string json = CreateJsonString(args);
        return JsonSerializer.Deserialize<T>(json)
            ?? throw new ArgumentException(
                "The given arguments did not produce a valid object");
    }

    // Get constructor parameters for all Vehicle classes
    public static void Test()
    {
        Type[] vehicleTypes = _currModule.FindTypes(ConcreteVehicleFilter,  null)
            ?? throw new InvalidFilterCriteriaException(
                "Could not find concrete vehicle types");
        foreach (Type vehicleType in vehicleTypes)
        {
            Console.WriteLine($"Vehicle: {vehicleType.Name}");
            ConstructorInfo[] constructors = vehicleType.GetConstructors();
            if (constructors.Length != 1)
            {
                Console.WriteLine(
                    $"  Expected 1 constructor, get {constructors.Length}");
                continue;
            }
            ConstructorInfo constructorInfo = constructors[0];
            ParameterInfo[] parameters = constructorInfo.GetParameters();
            foreach (ParameterInfo parameter in parameters)
            {
                Console.WriteLine($"  {parameter.ParameterType.Name} {parameter.Name}");
            }
        }
    }

    // Filter: Vehicles are non-abstract, non-interface classes that implement IVehicle
    private static bool ConcreteVehicleFilter(Type type, object? filterCriteria)
        => (!(type.IsAbstract || type.IsInterface)
            && type.GetInterfaces().Contains(typeof(IVehicle)));
}
