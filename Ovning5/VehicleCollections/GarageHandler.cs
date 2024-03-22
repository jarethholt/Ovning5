using Ovning5.Vehicles;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Ovning5.VehicleCollections;

public class GarageHandler(int maxCapacity) : IGarage<Vehicle>
{
    private readonly Garage<Vehicle> _garage = new(maxCapacity);

    public int Count => _garage.Count;

    public bool HasSpace => _garage.HasSpace;

    public int MaxCapacity => _garage.MaxCapacity;

    public bool FindByID(string vehicleID, [MaybeNullWhen(false)] out Vehicle vehicle)
        => _garage.FindByID(vehicleID, out vehicle);

    public IEnumerator<Vehicle?> GetEnumerator() => _garage.GetEnumerator();

    public string ListAll() => _garage.ListAll();

    public bool Remove(Vehicle vehicle) => _garage.Remove(vehicle);

    public bool TryAdd(Vehicle vehicle) => _garage.TryAdd(vehicle);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
