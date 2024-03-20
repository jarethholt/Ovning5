using Ovning5.Registration;
using Ovning5.Vehicles;
using System.Text.Json;

namespace Ovning5;

internal class Program
{
    static void Main()
    {
        VehicleExamples();
        RegistrationCodeExamples();
        RegistryExample();
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

    static void RegistrationCodeExamples()
    {
        Random random = new(12345);
        RegistrationCode[] registrationCodes =
        [
            RegistrationCode.GenerateCode(random),
            RegistrationCode.GenerateCode(random),
            RegistrationCode.GenerateCode(random),
            new RegistrationCode("ABC123"),
            new RegistrationCode("XYZ098"),
        ];

        Console.WriteLine("Some example registration codes:");
        foreach (RegistrationCode registrationCode in registrationCodes)
            Console.WriteLine(registrationCode);
        Console.WriteLine("---");
        Console.WriteLine();
    }

    private static void RegistryExample()
    {
        string[] codeStrings = ["ABC123", "XYZ098", "DEF567"];
        RegistrationCode[] codes
            = codeStrings.Select(codeString => new RegistrationCode(codeString))
                         .ToArray();
        Registry registry = new(codes);

        Console.WriteLine("Example of JSON-serialized vehicle registry:");
        string registryAsJson = registry.Serialize();
        Console.WriteLine(registryAsJson);
        Console.WriteLine();

        Console.WriteLine("Attempt to deserialize this JSON:");
        try
        {
            Registry test = Registry.Deserialize(registryAsJson);
            if (!registry.RegistrationCodes.SetEquals(test.RegistrationCodes))
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
}
