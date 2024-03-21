using Ovning5.Vehicles;
using System.Collections;

namespace Ovning5.VehicleCollections;

internal class Garage<T> : IEnumerable<T> where T : Vehicle
{
    private readonly T[] _vehicles;
    public int MaxCapacity { get; }
    public IEnumerable<T> Vehicles
    {
        get => _vehicles.Where(vehicle => vehicle is not null);
    }
    public int Count => Vehicles.Count();

    public Garage(int maxCapacity)
    {
        if (maxCapacity <= 0)
            throw new ArgumentOutOfRangeException(
                message: "A garage's capacity must be > 0",
                paramName: nameof(maxCapacity));
        MaxCapacity = maxCapacity;
        _vehicles = new T[MaxCapacity];
    }

    public Garage(int maxCapacity, IEnumerable<T> vehicles)
    {
        int numVehicles = vehicles.Count();
        if (numVehicles > maxCapacity)
            throw new ArgumentOutOfRangeException(
                message: $"Cannot initialize a garage with capacity {maxCapacity} "
                       + $"with {numVehicles} vehicles",
                paramName: nameof(vehicles));

        MaxCapacity = maxCapacity;
        _vehicles = new T[MaxCapacity];
        Array.Copy(vehicles.ToArray(), _vehicles, numVehicles);
    }

    public IEnumerator<T> GetEnumerator()
        => (IEnumerator<T>)_vehicles.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
