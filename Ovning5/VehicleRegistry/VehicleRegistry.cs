using Microsoft.Win32;
using Ovning5.Vehicles;
using System.Text.Json;

namespace Ovning5.VehicleRegistry;

public class VehicleRegistry<T> where T : Vehicle
{
    private readonly Dictionary<VehicleID, T> _vehicles = [];

    public VehicleRegistry() { }

    public VehicleRegistry(Dictionary<VehicleID, T> vehicles)
    {
        _vehicles = vehicles;
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

    public bool TryAdd(T vehicle)
        => _vehicles.TryAdd(vehicle.VehicleID, vehicle);

    public bool Remove(VehicleID vehicleID)
        => _vehicles.Remove(vehicleID);

    public bool Remove(T vehicle) => Remove(vehicle.VehicleID);

    public VehicleID GenerateNewID(Random random)
    {
        VehicleID vehicleID;
        bool isNew;
        do
        {
            vehicleID = VehicleID.GenerateID(random);
            isNew = Contains(vehicleID);
        } while (!isNew);
        return vehicleID;
    }

    public string Serialize()
        => JsonSerializer.Serialize(_vehicles);

    public static VehicleRegistry<T> Deserialize(string json)
    {
        var vehicles = JsonSerializer.Deserialize<Dictionary<VehicleID,T>>(json);
        return vehicles is null ? new VehicleRegistry<T>()
                                : new VehicleRegistry<T>(vehicles);
    }
}
