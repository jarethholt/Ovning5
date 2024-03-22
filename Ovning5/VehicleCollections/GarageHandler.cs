using Ovning5.Vehicles;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Ovning5.VehicleCollections;

public class GarageHandler(int maxCapacity) : IGarage<IVehicle>
{
    private readonly Garage<IVehicle> _garage = new(maxCapacity);

    public int Count => _garage.Count;

    public bool HasSpace => _garage.HasSpace;

    public int MaxCapacity => _garage.MaxCapacity;

    public bool FindByID(string vehicleID, [MaybeNullWhen(false)] out IVehicle vehicle)
        => _garage.FindByID(vehicleID, out vehicle);

    public IEnumerator<IVehicle?> GetEnumerator() => _garage.GetEnumerator();

    public string ListAll() => _garage.ListAll();

    public bool Remove(IVehicle vehicle) => _garage.Remove(vehicle);

    public bool TryAdd(IVehicle vehicle) => _garage.TryAdd(vehicle);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
