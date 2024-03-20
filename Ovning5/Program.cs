using Ovning5.Registration;
using Ovning5.Vehicles;

namespace Ovning5;

internal class Program
{
    static void Main()
    {
        VehicleExamples();
        RegistrationCodeExamples();
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

        foreach (Vehicle vehicle in vehicles)
            Console.WriteLine(vehicle);
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

        foreach (RegistrationCode registrationCode in registrationCodes)
            Console.WriteLine(registrationCode);
    }
}
