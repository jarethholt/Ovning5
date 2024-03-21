using Ovning5.VehicleRegistry;
using Ovning5.Vehicles;
using System.Reflection;
using System.Text.Json;

namespace Ovning5;

internal class Program
{
    static void Main()
    {
        VehicleExamples();
        VehicleIDExamples();
        RegistryExample();
        ReflectionTest();
    }

    static void VehicleExamples()
    {
        // Describe a few example vehicles
        Vehicle[] vehicles =
        [
            new Car(1, "Toyota", "Corolla", 2002, 1000),
            new Motorcycle(2, "Kawasaki", "Ninja ZX", 2024, "Sportbike"),
            new Bus(3, true),
            new Boat(4, 25),
            new Airplane(5, "Boeing", "747", "Passenger jet", 350),
        ];
        vehicles[0].PaintColor("Beige");
        vehicles[2].PaintColor("Red");

        Console.WriteLine("Some example vehicles:");
        foreach (Vehicle vehicle in vehicles)
            Console.WriteLine(vehicle);
        Console.WriteLine("---");
        Console.WriteLine();
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
            Console.WriteLine(vehicleID);
        Console.WriteLine("---");
        Console.WriteLine();
    }

    static void RegistryExample()
    {
        string[] codes = ["ABC123", "XYZ098", "DEF567"];
        VehicleID[] vehicleIDs
            = codes.Select(code => new VehicleID(code)).ToArray();
        Registry registry = new(vehicleIDs);

        Console.WriteLine("Example of JSON-serialized vehicle registry:");
        string registryAsJson = registry.Serialize();
        Console.WriteLine(registryAsJson);
        Console.WriteLine();

        Console.WriteLine("Attempt to deserialize this JSON:");
        try
        {
            Registry test = Registry.Deserialize(registryAsJson);
            if (!registry.VehicleIDs.SetEquals(test.VehicleIDs))
                Console.WriteLine("Deserialization succeeded but produced the wrong registry.");
            else
                Console.WriteLine("Deserialization succeeded and produced the correct registry!");
        }
        catch (JsonException ex)
        {
            Console.WriteLine("Deserialization failed and produced this error:");
            Console.WriteLine(ex.Message);
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
            if (method.Name.StartsWith("get_") || method.Name.StartsWith("set_"))
                continue;
            List<string> args = [];
            foreach (var param in method.GetParameters())
            {
                args.Add($"{param.ParameterType.Name} {param.Name}");
            }
            string paramdesc = string.Join(", ", args);
            Console.WriteLine($"  {method.ReturnType.Name} {method.Name}({paramdesc})");
        }
    }
}
