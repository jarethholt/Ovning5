using Ovning5.Vehicles;
using System.Diagnostics.CodeAnalysis;

namespace Ovning5.VehicleCollections;

public interface IGarage<T> : IEnumerable<T?> where T : class, IVehicle
{
    int Count { get; }
    bool HasSpace { get; }
    int MaxCapacity { get; }

    bool FindByID(string vehicleID, [MaybeNullWhen(false)] out T vehicle);

    string ListAll();

    bool Remove(T vehicle);

    bool TryAdd(T vehicle);
}
