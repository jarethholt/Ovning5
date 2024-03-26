using System.Text.Json.Serialization;

namespace Ovning5.Vehicles;

internal record Bus(
    VehicleID VehicleID,
    string Color,
    bool IsSingleCabin)
    : Vehicle(VehicleID, Color), IVehicle
{
    // JSON doesn't play nice with booleans, need an integer instead
    // See https://stackoverflow.com/a/49153148
    [JsonIgnore]
    public bool IsSingleCabin { get; set; } = IsSingleCabin;
    [JsonPropertyName(nameof(IsSingleCabin))]
    private int IsSingleCabinAsInt
    {
        get => IsSingleCabin ? 1 : 0;
        set => IsSingleCabin = value > 0;
    }
    // For a bus, NumberOfWheels is basically a method (requires an operation)
    // but can be declared as a property for consistency with IVehicle
    public override int NumberOfWheels
    {
        get => IsSingleCabin ? 6 : 10;
    }

    public static new Vehicle Example()
        => new Bus(
            new VehicleID("GHI789"),
            "Red",
            true);

    //public override string ToString()
    //{
    //    string cabins = IsSingleCabin ? "single-cabin" : "double-cabin";
    //    return $"{Color} {cabins} bus";
    //}
}
