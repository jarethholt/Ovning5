using Ovning5.UI;
using System.Reflection;

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
    private static readonly Type[] _vehicleTypes;
    private static readonly string[] _vehicleTypeNames;
    private static readonly Dictionary<string, ParameterSpecs>
        _constructorParams = [];
    private static readonly Dictionary<string, ParameterSpecs>
        _properties = [];

    public static string[] AvailableVehicles { get => _vehicleTypeNames; }
    public static Dictionary<string, ParameterSpecs> VehicleConstructorParams
    {
        get => _constructorParams;
    }
    public static Dictionary<string, ParameterSpecs> VehicleProperties
    {
        get => _properties;
    }

    static VehicleBuilder_orig()
    {
        var module = Assembly.GetExecutingAssembly().GetModules()[0];
        _vehicleTypes = module.FindTypes(ConcreteVehicleFilter, null);
        _vehicleTypeNames = _vehicleTypes.Select(type => type.Name).ToArray();
        foreach (Type vehicleType in _vehicleTypes)
        {
            ParameterInfo[] parameters
                = vehicleType.GetConstructors()[0].GetParameters();
            _constructorParams[vehicleType.Name]
                = parameters.Select(p => (p.Name!, p.ParameterType)).ToList();
            PropertyInfo[] properties
                = vehicleType.GetProperties();
            _properties[vehicleType.Name]
                = properties.Select(p => (p.Name, p.PropertyType)).ToList();
        }
    }

    public static Type AskForVehicleTypeName(IUI ui)
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
        IUI ui,
        IEnumerable<VehicleID>? vehicleIDs = null)
    {
        ui.WriteLine("Please provide the necessary parameters for the vehicle.");
        ParameterSpecs parameters = VehicleConstructorParams[vehicleType.Name];
        List<(string name, string value)> args = [];
        foreach ((string name, Type type) in parameters)
        {
            if (type.Equals(typeof(VehicleID)))
            {
                string prompt
                    = $"{name} (string with format {VehicleID.CodeFormat}; "
                    + "leave blank to generate random): ";
                VehicleID? result = Utilities.AskForVehicleID(
                    prompt, ui, isEmptyOk: true);
                VehicleID value;
                if (result is null)
                {
                    if (vehicleIDs is null)
                        value = VehicleID.GenerateID(new Random());
                    else
                        value = VehicleID.GenerateUniqueID(new Random(), vehicleIDs);
                }
                else
                    value = (VehicleID)result;
                args.Add((name, $"\"{value}\""));
            }
            else if (type.Equals(typeof(bool)))
            {
                string prompt = $"{name} ([y]es or [n]o): ";
                bool value = Utilities.AskForYesNo(prompt, ui);
                int valueAsInt = value ? 1 : 0;
                args.Add((name, valueAsInt.ToString()));
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

    public static object[] AskForConstructorParameters_native(
        Type vehicleType,
        IUI ui,
        IEnumerable<VehicleID>? vehicleIDs = null)
    {
        ui.WriteLine("Please provide the necessary parameters for the vehicle.");
        ParameterSpecs parameters = VehicleConstructorParams[vehicleType.Name];
        object[] args = new object[parameters.Count];
        for (int i = 0; i < parameters.Count; i++)
        {
            (string name, Type type) = parameters[i];
            object value;
            if (type.Equals(typeof(VehicleID)))
            {
                string prompt
                    = $"{name} (string with format {VehicleID.CodeFormat}; "
                    + "leave blank to generate random): ";
                VehicleID? result = Utilities.AskForVehicleID(
                    prompt, ui, isEmptyOk: true);
                if (result is null)
                {
                    if (vehicleIDs is null)
                        value = VehicleID.GenerateID(new Random());
                    else
                        value = VehicleID.GenerateUniqueID(new Random(), vehicleIDs);
                }
                else
                    value = (VehicleID)result;
            }
            else if (type.Equals(typeof(bool)))
            {
                string prompt = $"{name} ([y]es or [n]o): ";
                value = Utilities.AskForYesNo(prompt, ui);
            }
            else if (type.Equals(typeof(int)))
            {
                string prompt = $"{name} (positive int): ";
                value = (int)Utilities.AskForPositiveInt(prompt, ui)!;
            }
            else if (type.Equals(typeof(string)))
            {
                string prompt = $"{name} (string): ";
                value = (string)Utilities.AskForString(prompt, ui)!;
            }
            else
            {
                throw new ArgumentException(
                    $"Parameter {name} has a type {type.Name} that was unhandled. "
                    + "Expected one of: VehicleID, bool, int, or string");
            }
            args[i] = value;
        }
        return args;
    }

    public static IVehicle? BuildVehicle(Type vehicleType, object[] args)
        => Activator.CreateInstance(vehicleType, args) as IVehicle;

    public static IVehicle? ConstructVehicle(IUI ui, IEnumerable<VehicleID>? vehicleIDs = null)
    {
        Type vehicleType = AskForVehicleTypeName(ui);
        object[] args = AskForConstructorParameters_native(vehicleType, ui, vehicleIDs: vehicleIDs);
        return BuildVehicle(vehicleType, args);
    }

    // Get constructor parameters for all Vehicle classes
    public static void Test()
    {
        foreach (Type vehicleType in _vehicleTypes)
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
            PropertyInfo[] properties = vehicleType.GetProperties();
            Console.WriteLine("Vehicle properties:");
            foreach (PropertyInfo property in properties)
            {
                Console.WriteLine($"  {property.PropertyType.Name} {property.Name}");
            }
        }
    }

    // Filter: Vehicles are non-abstract, non-interface classes that implement IVehicle
    private static bool ConcreteVehicleFilter(Type type, object? filterCriteria)
        => (!(type.IsAbstract || type.IsInterface)
            && type.GetInterfaces().Contains(typeof(IVehicle)));
}
