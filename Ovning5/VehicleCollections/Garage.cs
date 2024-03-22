using Ovning5.Vehicles;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Ovning5.VehicleCollections;

internal class Garage<T> : IGarage<T> where T : class, IVehicle
{
    private readonly T?[] _vehicles;
    public int MaxCapacity { get; }
    public int Count => _vehicles.Count(v => v is not null);
    public bool HasSpace => _vehicles.Any(vehicle => vehicle is null);

    public Garage(int maxCapacity)
    {
        if (maxCapacity <= 0)
            throw new ArgumentOutOfRangeException(
                message: "A garage's capacity must be > 0",
                paramName: nameof(maxCapacity));
        MaxCapacity = maxCapacity;
        _vehicles = new T?[MaxCapacity];
        Array.Fill(_vehicles, null);
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
        _vehicles = new T?[MaxCapacity];
        Array.Fill(_vehicles, null);
        Array.Copy(vehicles.ToArray(), _vehicles, numVehicles);
    }

    public bool TryAdd(T vehicle)
    {
        if (!HasSpace)
            return false;
        int index = Array.FindIndex(_vehicles, vehicle => vehicle is null);
        _vehicles[index] = vehicle;
        return true;
    }

    public bool Remove(T vehicle)
    {
        int index = Array.FindIndex(_vehicles, vehicle.Equals);
        if (index == -1)
            return false;
        _vehicles[index] = null;
        return true;
    }

    public string ListAll()
    {
        StringBuilder stringBuilder = new();
        foreach (var vehicle in _vehicles)
        {
            if (vehicle is not null)
                stringBuilder.AppendLine(vehicle.ToString());
        }
        return stringBuilder.ToString();
    }

    public bool FindByID(string vehicleID, [MaybeNullWhen(false)] out T vehicle)
    {
        try
        {
            vehicle = _vehicles.First(
                v => v is not null && v.VehicleID.Equals(vehicleID))!;
            return true;
        }
        catch (InvalidOperationException)
        {
            vehicle = null;
            return false;
        }
    }

    public IEnumerator<T?> GetEnumerator()
        => (IEnumerator<T?>)_vehicles.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
