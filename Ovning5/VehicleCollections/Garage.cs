using Ovning5.Vehicles;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Ovning5.VehicleCollections;

/* Internally, the Garage is represented by the array T?[] _vehicles.
 * An empty space has a value of null; an occupied space has an IVehicle
 * class instance.
 */
internal class Garage<T> : IGarage<T> where T : class, IVehicle
{
    private readonly T?[] _vehicles;
    public int MaxCapacity { get; }
    public int Count => _vehicles.Count(vehicle => vehicle is not null);
    public bool HasSpace => _vehicles.Any(vehicle => vehicle is null);

    // Initialize a garage with a given capacity by setting _vehicles
    // to the correct size of all-null values
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

    // Initialize a garage with an existing collection of vehicles
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

    // Try to add (park) and remove vehicles
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
        // FindIndex returns -1 when no match is found
        if (index == -1)
            return false;
        _vehicles[index] = null;
        return true;
    }

    public bool RemoveByID(string vehicleID)
    {
        if (!FindByID(vehicleID, out T? vehicle))
            return false;
        return Remove(vehicle);
    }

    // List all vehicles by simply showing their JSON
    public string ListAll()
    {
        StringBuilder stringBuilder = new();
        int i = 1;
        foreach (var vehicle in _vehicles)
        {
            if (vehicle is not null)
                stringBuilder.AppendLine($"{i++}: {vehicle.ToString()}");
        }
        return stringBuilder.ToString();
    }

    // Summarize by printing out the number of each type of vehicle
    public string VehicleSummary()
    {
        if (Count == 0)
            return "Empty";
        Dictionary<string, int> vehicleCounts = new()
        {
            { "Airplane", 0 },
            { "Boat", 0 },
            { "Bus", 0 },
            { "Car", 0 },
            { "Motorcycle", 0 },
        };
        foreach (var vehicle in _vehicles)
        {
            if (vehicle is null)
                continue;
            vehicleCounts[vehicle.GetType().Name]++;
        }

        List<string> vehicleStrings = [];
        foreach (var kvp in vehicleCounts)
        {
            string vehicleTypeName = kvp.Key;
            int count = kvp.Value;
            if (count == 0)
                continue;
            string suffix = "";
            if (count > 1)
                suffix = vehicleTypeName.Equals("Bus") ? "es" : "s";
            vehicleStrings.Add($"{count} {vehicleTypeName}{suffix}");
        }
        return string.Join(", ", vehicleStrings);
    }

    public bool FindByID(string vehicleID, [MaybeNullWhen(false)] out T vehicle)
    {
        try
        {
            vehicle = _vehicles.First(
                vehicle => vehicle is not null && vehicle.VehicleID.Equals(vehicleID))!;
            return true;
        }
        catch (InvalidOperationException)
        {
            vehicle = null;
            return false;
        }
    }

    public IEnumerable<VehicleID> ListVehicleIDs()
    {
        if (Count == 0)
            return [];
        return _vehicles
            .Where(vehicle => vehicle is not null)
            .Select(vehicle => vehicle!.VehicleID);
    }

    public IEnumerator<T?> GetEnumerator()
        => _vehicles.OfType<T?>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static Garage<T> Example() => throw new NotImplementedException();
}
