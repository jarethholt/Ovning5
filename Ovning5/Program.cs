using Ovning5.Vehicles;

namespace Ovning5;

internal class Program
{
    static void Main()
    {
        // Describe a few example vehicles
        Vehicle[] vehicles =
        [
            new Car(1, "Toyota", "Corolla", 2002),
            new Motorcycle(2, "Kawasaki", "Ninja ZX", 2024, "Sportbike"),
            new Bus(3, true),
        ];
        vehicles[0].PaintColor("Beige");
        vehicles[2].PaintColor("Red");

        foreach (Vehicle vehicle in vehicles)
            Console.WriteLine(vehicle);
    }
}
