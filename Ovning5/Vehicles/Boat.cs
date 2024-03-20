namespace Ovning5.Vehicles;

public class Boat : Vehicle
{
    private const float _metersToFeet = 3.28084f;

    public override int NumberOfWheels { get; } = 0;
    public override string Color { get; protected set; } = "White";
    public int LengthInMeters { get; }
    public int LengthInFeet => (int)(LengthInMeters * _metersToFeet);
    public string Name { get; private set; } = "(Unnamed)";

    public Boat(int registrationNumber, int lengthInMeters)
    {
        RegistrationNumber = registrationNumber;
        LengthInMeters = lengthInMeters;
    }

    public void ChristenBoat(string name) => Name = name;

    public override string ToString() => $"{LengthInMeters}-m {Color} boat: {Name}";
}
