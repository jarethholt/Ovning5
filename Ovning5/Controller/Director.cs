using Ovning5.UI;
using Ovning5.VehicleCollections;
using Ovning5.VehicleFactories;
using Ovning5.Vehicles;

namespace Ovning5.Controller;

internal class Director(IUI ui)
{
    private readonly IUI _ui = ui;
    private readonly VehiclePlant _vehiclePlant = new();
    private readonly Dictionary<string, GarageHandler> _garages = [];
    private string? _currentGarageName;
    private GarageHandler? _currentGarage;

    private bool NoGarageSelected()
    {
        if (_currentGarage is null)
        {
            _ui.WriteLine("No garage has been selected. Use ChooseGarage first.");
            _ui.Write("Press enter to continue.");
            _ = _ui.ReadInput();
            return true;
        }
        return false;
    }

    public void ChooseGarage()
    {
        _ui.Clear();
        if (_garages.Count == 0)
        {
            _ui.WriteLine("No garages are available. Use AddNewGarage first.");
            _ui.Write("Press enter to go back to the menu.");
            _ = _ui.ReadInput();
            return;
        }

        _ui.WriteLine("The following garages are available:");
        foreach (var kvp in _garages)
            _ui.WriteLine($"  {kvp.Key}: {kvp.Value.MaxCapacity} spaces");
        _ui.WriteLine();
        _currentGarageName = Utilities.AskForDictKey(
            "Please enter a garage name: ",
            _garages,
            _ui);
        _currentGarage = _garages[_currentGarageName];
        _ui.Write($"Garage {_currentGarageName} selected. Press enter to continue.");
        _ = _ui.ReadInput();
    }

    private void AddNewGarage_Single()
    {
        _ui.Clear();
        int numSpaces = Utilities.AskForPositiveInt(
            "Number of spaces for the garage: ",
            _ui);
        string name = Utilities.AskForString("A nickname for the garage: ", _ui);
        try
        {
            var garage = new GarageHandler(numSpaces);
            _garages.Add(name, garage);
            _ui.WriteLine($"\nAdded garage {name} with {numSpaces} spaces.");
        }
        catch (Exception ex)
        {
            _ui.WriteLine($"\nCould not add this garage; got exception {ex}");
        }
    }

    public void AddNewGarage()
        => Utilities.Loop(AddNewGarage_Single, "Add another garage (y/n)? ", _ui);

    public void ViewGarage()
    {
        _ui.Clear();
        if (NoGarageSelected())
            return;
        _ui.WriteLine(_currentGarage!.ListAll());

        _ui.Write("Press enter to continue.");
        _ = _ui.ReadInput();
    }

    private void AddVehicleToGarage_Single()
    {
        _ui.Clear();
        if (NoGarageSelected())
            return;
        if (!_currentGarage!.HasSpace)
        {
            _ui.WriteLine(
                "This garage has no space. Use RemoveVehicleFromGarage first.");
            return;
        }
        IVehicleFactory vehicleFactory = _vehiclePlant.ChooseVehicleFactory(_ui);
        IVehicle vehicle = vehicleFactory.BuildVehicle(_ui);
        if (!_currentGarage.TryAdd(vehicle))
            _ui.WriteLine(
                "An error occurred while trying to add the vehicle to the garage");
        else
            _ui.WriteLine(
                $"Vehicle with ID {vehicle.VehicleID} was parked in the garage.");
    }

    public void AddVehicleToGarage()
        => Utilities.Loop(AddVehicleToGarage_Single, "Add another vehicle (y/n)? ", _ui);

    private void RemoveVehicleFromGarage_Single()
    {
        _ui.Clear();
        if (NoGarageSelected())
            return;
        if (_currentGarage!.Count == 0)
        {
            _ui.WriteLine("The current garage is empty; cannot remove a vehicle.");
            return;
        }

        _ui.WriteLine("The garage currently has these vehicles:");
        _ui.WriteLine(_currentGarage.ListAll());

        VehicleID vehicleID = Utilities.AskForVehicleID(
            "Please enter the ID of one of these vehicles: ",
            _ui);
        if (!_currentGarage.RemoveByID(vehicleID.Code))
            _ui.WriteLine(
                $"Failed to remove the vehicle with ID {vehicleID}; "
                + "are you sure it was parked?");
        else
            _ui.WriteLine(
                $"Vehicle with ID {vehicleID} was removed from the garage.");
    }

    public void RemoveVehicleFromGarage()
        => Utilities.Loop(RemoveVehicleFromGarage_Single, "Remove another vehicle (y/n)? ", _ui);
}
