using Ovning5.Vehicles;
using System.Diagnostics.CodeAnalysis;

namespace Ovning5.VehicleCollections;

/* Like with Vehicle, there seemed to be no benefit to having both an
 * interface and a generic implementation until the static method
 * Example was added. Then I could only use the actual Garage class
 * (or more often, the non-generic GarageHandler) in place of a generic.
 */
internal interface IGarage<T> : IEnumerable<T?> where T : class, IVehicle
{
    // Properties: Current number of vehicles, whether there is more space,
    // and number of spaces
    int Count { get; }
    bool HasSpace { get; }
    int MaxCapacity { get; }

    // List all VehicleIDs
    IEnumerable<VehicleID> ListVehicleIDs();

    // Find a vehicle by its ID
    bool FindByID(string vehicleID, [MaybeNullWhen(false)] out T vehicle);

    // Print all vehicles currently in the garage
    string ListAll();

    // Summarize the number of each vehicle type in the garage
    string VehicleSummary();

    // Try to add or remove a vehicle from the garage
    bool Remove(T vehicle);

    bool TryAdd(T vehicle);

    // Provides an example garage (for default purposes)
    static abstract Garage<T> Example();
}
