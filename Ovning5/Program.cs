﻿using Ovning5.Registry;
using Ovning5.VehicleCollections;
using Ovning5.Vehicles;

namespace Ovning5;

internal class Program
{
    static void Main()
    {
        VehicleExamples();
        GarageExample();
        VehicleIDExamples();
        VehicleRegistryExample();
        ReflectionTest();
    }

    static void VehicleExamples()
    {
        // Describe a few example vehicles
        Vehicle[] vehicles =
        [
            new Car(new VehicleID("ABC123"), "Beige", "Toyota", "Corolla", 2002, 1000),
            new Motorcycle(new VehicleID("DEF456"), "Black", "Kawasaki", "Ninja ZX", 2024, "Sportbike"),
            new Bus(new VehicleID("GHI789"), "Red", true),
            new Boat(new VehicleID("JKL012"), "White", 25, "Calculon's Pride"),
            new Airplane(new VehicleID("MNO345"), "White", "Boeing", "747", "Passenger jet", 350),
        ];

        Console.WriteLine("Some example vehicles:");
        foreach (Vehicle vehicle in vehicles)
            Console.WriteLine(vehicle);
        Console.WriteLine("---");
        Console.WriteLine();
    }

    private static void GarageExample()
    {
        Vehicle[] vehicles =
        [
            new Car(new VehicleID("ABC123"), "Beige", "Toyota", "Corolla", 2002, 1000),
            new Motorcycle(new VehicleID("DEF456"), "Black", "Kawasaki", "Ninja ZX", 2024, "Sportbike"),
            new Bus(new VehicleID("GHI789"), "Red", true),
            new Boat(new VehicleID("JKL012"), "White", 25, "Calculon's Pride"),
            new Airplane(new VehicleID("MNO345"), "White", "Boeing", "747", "Passenger jet", 350),
        ];
        Garage<Vehicle> garage = new(4);

        for (int i = 0; i < vehicles.Length; i++)
        {
            var vehicle = vehicles[i];
            bool success = garage.TryAdd(vehicle);
            if (!success)
                Console.WriteLine(
                    $"Could not add vehicle #{i + 1} to the garage; "
                    + $"its capacity is {garage.MaxCapacity}");
            else
                Console.WriteLine($"Added vehicle #{i + 1}: {vehicle}");
        }

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

    static void VehicleRegistryExample()
    {
        Random random = new(12345);
        VehicleRegistry registry = new();

        VehicleID vehicleID;
        Vehicle vehicle;
        vehicleID = registry.GenerateNewID(random);
        vehicle = new Car(vehicleID, "Beige", "Toyota", "Corolla", 2002, 1000);
        if (!registry.TryAdd(vehicle))
            Console.WriteLine($"Could not add this vehicle: {vehicle}");
        vehicleID = registry.GenerateNewID(random);
        vehicle = new Motorcycle(vehicleID, "Black", "Kawasaki", "Ninja ZX", 2024, "Sportbike");
        if (!registry.TryAdd(vehicle))
            Console.WriteLine($"Could not add this vehicle: {vehicle}");
        vehicleID = registry.GenerateNewID(random);
        vehicle = new Bus(vehicleID, "Red", true);
        if (!registry.TryAdd(vehicle))
            Console.WriteLine($"Could not add this vehicle: {vehicle}");

        Console.WriteLine("Vehicles in the registry:");
        foreach (Vehicle item in registry)
        {
            Console.WriteLine($"  {item}");
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
    }
}
