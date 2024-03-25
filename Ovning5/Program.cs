using Ovning5.VehicleCollections;
using Ovning5.Vehicles;
using System.Text.Json;

namespace Ovning5;

internal class Program
{
    static readonly Vehicle[] _vehicles =
    [
        Car.Example(),
        Motorcycle.Example(),
        Bus.Example(),
        Boat.Example(),
        Airplane.Example(),
    ];

    static void Main()
    {
        VehicleIDExamples();
        VehicleExamples();
        GarageExample();
        ReflectionTest();
        VehicleBuilder_orig.Test();
        SelectParamsTest();
    }

    static void VehicleIDExamples()
    {
        Random random = new(12345);
        VehicleID[] vehicleIDs =
        [
            VehicleID.GenerateID(random),
            VehicleID.GenerateID(random),
            VehicleID.GenerateID(random),
            new VehicleID("ABC123"),
            new VehicleID("XYZ098"),
        ];

        Console.WriteLine("Some example registration codes:");
        foreach (VehicleID vehicleID in vehicleIDs)
        {
            Console.WriteLine(vehicleID);
            string vehicleIDAsJson = JsonSerializer.Serialize(vehicleID);
            Console.WriteLine(vehicleIDAsJson);
            try
            {
                VehicleID test = JsonSerializer.Deserialize<VehicleID>(vehicleIDAsJson);
                if (vehicleID.Equals(test))
                    Console.WriteLine("Deserialization succeeded and was correct!");
                else
                    Console.WriteLine($"Deserialization succeeded but was incorrect: {test}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Deserialization failed and produced the following: {ex.Message}");
            }
        }
        Console.WriteLine("---");
        Console.WriteLine();
    }

    static void VehicleExamples()
    {
        Console.WriteLine("Some example vehicles:");
        foreach (var vehicle in _vehicles)
        {
            Type vehicleType = vehicle.GetType();
            Console.WriteLine(vehicle);
            string vehicleAsJson = JsonSerializer.Serialize(vehicle, vehicleType);
            Console.WriteLine(vehicleAsJson);
            object? result = JsonSerializer.Deserialize(vehicleAsJson, vehicleType);
            var test = Convert.ChangeType(result, vehicleType);
            if (!vehicle.Equals(test))
            {
                Console.WriteLine("Vehicle was not deserialized correctly...");
                Console.WriteLine(test);
            }
            else
                Console.WriteLine("Vehicle was deserialized correctly!");
        }
        Console.WriteLine("---");
        Console.WriteLine();
    }

    private static void GarageExample()
    {
        Garage<Vehicle> garage = new(4);

        for (int i = 0; i < _vehicles.Length; i++)
        {
            var vehicle = _vehicles[i];
            bool success = garage.TryAdd(vehicle);
            if (!success)
                Console.WriteLine(
                    $"Could not add vehicle #{i + 1} to the garage; "
                    + $"its capacity is {garage.MaxCapacity}");
        }
        Console.WriteLine(garage.ListAll());

        string[] vehicleIDs = ["GHI789", "XXX000"];
        foreach (var vehicleID in vehicleIDs)
        {
            Console.WriteLine($"Looking for vehicle {vehicleID} in the garage");
            if (garage.FindByID(vehicleID, out Vehicle? vehicle))
                Console.WriteLine($"  Found: {vehicle}");
            else
                Console.WriteLine($"  Could not find {vehicleID}");
        }

        Console.WriteLine("---");
        Console.WriteLine();
    }

    static void ReflectionTest()
    {
        Type type = typeof(Car);
        Console.WriteLine($"Examining reflection properties of type {type}");
        Console.WriteLine();

        var constructors = type.GetConstructors();
        if (constructors.Length != 1)
        {
            Console.WriteLine($"Expected 1 constructor, got {constructors.Length}");
            return;
        }
        var constructorInfo = constructors[0];
        var paramInfo = constructorInfo.GetParameters();
        Console.WriteLine("Parameters for the constructor:");
        foreach (var info in paramInfo)
            Console.WriteLine($"  {info.ParameterType.Name} {info.Name}");

        var properties = type.GetProperties();
        Console.WriteLine("Properties of this type:");
        foreach (var prop in properties)
        {
            string desc = $"  {prop.PropertyType.Name} {prop.Name}: ";
            List<string> terms = [];
            var getMethod = prop.GetMethod;
            if (getMethod is not null && getMethod.IsPublic)
                terms.Add("get");
            var setMethod = prop.SetMethod;
            if (setMethod is not null && setMethod.IsPublic)
                terms.Add("set");
            desc += string.Join(", ", terms);
            Console.WriteLine(desc);
        }

        var methods = type.GetMethods();
        Console.WriteLine("Available methods:");
        foreach (var method in methods)
        {
            if (method.Name.StartsWith("get_")
                || method.Name.StartsWith("set_")
                || method.Name.StartsWith("op_"))
                continue;
            List<string> args = [];
            foreach (var param in method.GetParameters())
            {
                args.Add($"{param.ParameterType.Name} {param.Name}");
            }
            string paramdesc = string.Join(", ", args);
            Console.WriteLine($"  {method.ReturnType.Name} {method.Name}({paramdesc})");
        }

        Console.WriteLine("---");
        Console.WriteLine();
    }

    private static void SelectParamsTest()
    {
        string vehicleTypeName = "Airplane";
        Console.WriteLine($"Examining constructor parameters for {vehicleTypeName}");
        Type vehicleType = VehicleBuilder_orig.GetVehicleType(vehicleTypeName);
        List<(string name, Type type)> paramList
            = VehicleBuilder_orig.GetConstructorParameters(vehicleType);

        foreach ((string name, Type type) in paramList)
        {
            Console.WriteLine($"  {type.Name} {name}");
        }
    }
}
