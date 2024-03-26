using Ovning5.Vehicles;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Ovning5.VehicleCollections;

// This class simply holds a Garage instance and provides its public members
internal class GarageHandler(int maxCapacity) : IGarage<Vehicle>
{
    private readonly Garage<Vehicle> _garage = new(maxCapacity);

    public int Count => _garage.Count;
    public bool HasSpace => _garage.HasSpace;
    public int MaxCapacity => _garage.MaxCapacity;

    public bool FindByID(string vehicleID, [MaybeNullWhen(false)] out Vehicle vehicle)
        => _garage.FindByID(vehicleID, out vehicle);

    public IEnumerator<Vehicle?> GetEnumerator() => _garage.GetEnumerator();

    public string ListAll() => _garage.ListAll();

    public string VehicleSummary() => _garage.VehicleSummary();

    public IEnumerable<VehicleID> ListVehicleIDs() => _garage.ListVehicleIDs();

    public bool Remove(Vehicle vehicle) => _garage.Remove(vehicle);

    public bool RemoveByID(string vehicleID) => _garage.RemoveByID(vehicleID);

    public bool TryAdd(Vehicle vehicle) => _garage.TryAdd(vehicle);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Example garage: 10 max spaces, filled with each Vehicle's Example()
    public static GarageHandler Example()
    {
        GarageHandler garage = new(10);
        garage.TryAdd(Car.Example());
        garage.TryAdd(Motorcycle.Example());
        garage.TryAdd(Bus.Example());
        garage.TryAdd(Boat.Example());
        garage.TryAdd(Airplane.Example());
        return garage;
    }

    static Garage<Vehicle> IGarage<Vehicle>.Example()
    {
        Garage<Vehicle> garage = new(10);
        garage.TryAdd(Car.Example());
        garage.TryAdd(Motorcycle.Example());
        garage.TryAdd(Bus.Example());
        garage.TryAdd(Boat.Example());
        garage.TryAdd(Airplane.Example());
        return garage;
    }
}
