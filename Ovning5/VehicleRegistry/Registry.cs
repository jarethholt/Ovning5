using System.Text.Json;

namespace Ovning5.VehicleRegistry;

public class Registry
{
    public HashSet<VehicleID> VehicleIDs { get; init; }

    public Registry() => VehicleIDs = [];

    public Registry(IEnumerable<VehicleID> vehicleIDs)
        => VehicleIDs = new(vehicleIDs);

    public bool Contains(VehicleID vehicleID)
        => VehicleIDs.Contains(vehicleID);

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

    public bool Add(VehicleID vehicleID)
        => VehicleIDs.Add(vehicleID);

    public bool Remove(VehicleID vehicleID)
        => VehicleIDs.Remove(vehicleID);

    public VehicleID GenerateAndAddNewCode(Random random)
    {
        VehicleID vehicleID;
        bool isNew;
        do
        {
            vehicleID = VehicleID.GenerateID(random);
            isNew = Contains(vehicleID);
        } while (!isNew);

        VehicleIDs.Add(vehicleID);
        return vehicleID;
    }

    public string Serialize() => JsonSerializer.Serialize(VehicleIDs.ToList());

    public static Registry Deserialize(string json)
    {
        var codes = JsonSerializer.Deserialize<List<VehicleID>>(json);
        return codes is null ? new Registry() : new Registry(codes);
    }
}
