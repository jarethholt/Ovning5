using Ovning5.Vehicles;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Ovning5.Registry;

public class VehicleRegistry : IEnumerable<Vehicle>
{
    private readonly Dictionary<VehicleID, Vehicle> _vehicles = [];

    public int Count => _vehicles.Count;

    public VehicleRegistry() { }

    public VehicleRegistry(Dictionary<VehicleID, Vehicle> vehicles)
        => _vehicles = vehicles;

    public VehicleRegistry(Dictionary<string, Vehicle> vehicles)
    {
        _vehicles = vehicles.Select(
            kvp => new KeyValuePair<VehicleID, Vehicle>(
                       new VehicleID(kvp.Key), kvp.Value)
                   ).ToDictionary();
    }

    public bool Contains(VehicleID vehicleID)
        => _vehicles.ContainsKey(vehicleID);

    public bool Contains(string code)
    {
        try
        {
            VehicleID vehicleID = new(code);
            return Contains(vehicleID);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public bool TryAdd(Vehicle vehicle)
        => _vehicles.TryAdd(vehicle.VehicleID, vehicle);

    public bool Remove(VehicleID vehicleID)
        => _vehicles.Remove(vehicleID);

    public bool Remove(Vehicle vehicle) => Remove(vehicle.VehicleID);

    public bool TryGetValue(
        VehicleID vehicleID,
        [MaybeNullWhen(false)] out Vehicle vehicle)
        => _vehicles.TryGetValue(vehicleID, out vehicle);

    public bool TryGetValue(
        string code,
        [MaybeNullWhen(false)] out Vehicle vehicle)
    {
        try
        {
            VehicleID vehicleID = new(code);
            return TryGetValue(vehicleID, out vehicle);
        }
        catch (FormatException)
        {
            vehicle = null;
            return false;
        }
    }

    public VehicleID GenerateNewID(Random random)
    {
        VehicleID vehicleID;
        bool isNew;
        do
        {
            vehicleID = VehicleID.GenerateID(random);
            isNew = !Contains(vehicleID);
        } while (!isNew);
        return vehicleID;
    }

    public bool Equals(VehicleRegistry other)
    {
        if (this.Count != other.Count)
            return false;
        return _vehicles.All(
            kvp => other.TryGetValue(kvp.Key, out var value)
                   && kvp.Value.Equals(value));
    }

    public IEnumerator<Vehicle> GetEnumerator()
        => _vehicles.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
