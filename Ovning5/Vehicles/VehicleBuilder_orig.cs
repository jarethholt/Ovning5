using Ovning5.UI;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ovning5.Vehicles;

public static class VehicleBuilder_orig
{
    private static readonly Module _currModule = typeof(VehicleBuilder_orig).Module;
    private static readonly Type[] _vehicleTypes
        = _currModule.FindTypes(ConcreteVehicleFilter, null);
    private static readonly string[] _vehicleTypeNames
        = _vehicleTypes.Select(type => type.Name).ToArray();

    public static string[] AvailableVehicles { get => _vehicleTypeNames; }

    public static IVehicle ConstructVehicle(IUI ui)
    {
        Type vehicleType = AskForVehicleType(ui);
        List<(string name, string value)> args
            = AskForConstructorParameters(vehicleType, ui);
        string vehicleAsJson = CreateJsonString(args);

        MethodInfo method = _currModule.GetMethod("JsonSerializer.Deserialize")
            ?? throw new Exception("Could not find the function 'JsonSerializer.Deserialize'");
        method = method.MakeGenericMethod(vehicleType);
        var result = method.Invoke(null, [vehicleAsJson]);
        var test = (typeof(vehicleType))result;
    }

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

    public static List<(string name, string value)> AskForConstructorParameters(
        Type vehicleType,
        IUI ui)
    {
        ui.WriteLine("Please provide the necessary parameters for the vehicle.");
        List<(string name, Type type)> parameters = GetConstructorParameters(vehicleType);
        List<(string name, string value)> args = [];
        foreach ((string name, Type type) in parameters)
        {
            if (type.Equals(typeof(VehicleID)))
            {
                string prompt = $"{name} (string with format {VehicleID.CodeFormat}): ";
                string value = Utilities.AskForVehicleID(prompt, ui);
                args.Add((name, $"\"{value.ToUpper()}\""));
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

    public static Type GetVehicleType(string vehicleTypeName)
    {
        return _vehicleTypes.FirstOrDefault(type => type.Name == vehicleTypeName)
               ?? throw new ArgumentOutOfRangeException(
                   nameof(vehicleTypeName),
                   $"Could not find {vehicleTypeName} among the known vehicle types: "
                   + $"{string.Join(", ", AvailableVehicles)}");
    }

    public static List<(string name, Type type)> GetConstructorParameters(Type vehicleType)
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

    public static T ConstructFromArgs<T>(IEnumerable<(string name, string value)> args) where T : class, IVehicle
    {
        string json = CreateJsonString(args);
        return JsonSerializer.Deserialize<T>(json)
            ?? throw new ArgumentException(
                "The given arguments did not produce a valid object");
    }

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
                Console.WriteLine($"  Expected 1 constructor, get {constructors.Length}");
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

    private static bool ConcreteVehicleFilter(Type type, object? filterCriteria)
        => (!(type.IsAbstract || type.IsInterface)
            && type.GetInterfaces().Contains(typeof(IVehicle)));
}
