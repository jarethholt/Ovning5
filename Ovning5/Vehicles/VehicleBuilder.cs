using System.Reflection;

namespace Ovning5.Vehicles;

public class VehicleBuilder
{
    private static readonly Module _currModule = typeof(VehicleBuilder).Module;
    private static readonly Type[] _vehicleTypes
        = _currModule.FindTypes(ConcreteVehicleFilter, null);

    public static string[] AvailableVehicles
    {
        get => _vehicleTypes.Select(type => type.Name).ToArray();
    }

    public static Type GetVehicleType(string vehicleTypeName)
    {
        return _vehicleTypes.FirstOrDefault(type => type.Name == vehicleTypeName)
               ?? throw new ArgumentOutOfRangeException(
                   nameof(vehicleTypeName),
                   $"Could not find {vehicleTypeName} among the known vehicle types: "
                   + $"{string.Join(", ", _vehicleTypes.Select(type => type.Name))}");
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
