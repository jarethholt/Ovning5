﻿namespace Ovning5.Vehicles;

internal record Motorcycle(
    VehicleID VehicleID,
    string Color,
    string Make,
    string Model,
    int Year,
    string MotorcycleType
) : Vehicle(VehicleID, Color)
{
    public override int NumberOfWheels { get; } = 2;

    //public override string ToString() => $"{Color} {Year} {Make} {Model}";
}
