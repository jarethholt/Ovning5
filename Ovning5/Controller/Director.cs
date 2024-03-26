using Ovning5.UI;
using Ovning5.VehicleCollections;
using Ovning5.VehicleFactories;
using Ovning5.Vehicles;

namespace Ovning5.Controller;

// Director controlling communication between UI and Garage
internal class Director
{
    private readonly IUI _ui;
    // VehiclePlant instance for producing Vehicles
    private readonly VehiclePlant _vehiclePlant = new();
    // List of known/registered garages
    private readonly Dictionary<string, GarageHandler> _garages = new()
    {
        { "Example", GarageHandler.Example() },
    };
    // Name and instance for the currently-selected garage
    private string _currentGarageName;
    private GarageHandler _currentGarage;
    // The main menu options: name, description, Action
    private readonly Options _options;

    public Director(IUI ui)
    {
        _ui = ui;
        // Set the current garage to the example
        _currentGarageName = "Example";
        _currentGarage = _garages[_currentGarageName];

        // Initialize the main menu options
        _options =
        [
            ("Quit", "Quit the program", Quit),
            ("AddNewGarage", "Create a new garage", AddNewGarage),
            ("ViewAllGarages", "View a summary of all garages", ViewAllGarages),
            ("ChooseGarage", "Select a garage to work with", ChooseGarage),
            ("ViewSelectedGarage", "View the vehicles in the current garage",
                ViewSelectedGarage),
            ("AddVehicleToGarage",
                "Add a vehicle to the current garage", AddVehicleToGarage),
            ("RemoveVehicleFromGarage",
                "Remove a vehicle from the current garage", RemoveVehicleFromGarage),
        ];
    }

    public void MainMenu()
    {
        // Loop until broken by Quit()
        while (true)
        {
            _ui.Clear();
            _ui.WriteLine("Main menu\n");

            _ui.DisplayOptions(_options);
            _ui.WriteLine($"Current garage: {_currentGarageName}");
            _ui.WriteLine();

            Action action = Utilities.AskForOption(
                "Choose an option (by name or number): ", _options, _ui);
            action.Invoke();
        }
    }

    // Quit the program
    public void Quit() => Environment.Exit(0);

    public void ChooseGarage()
    {
        _ui.Clear();
        _ui.WriteLine("Choose a garage\n");

        _ui.WriteLine("The following garages are available:");
        foreach (var kvp in _garages)
            _ui.WriteLine($"  {kvp.Key}: {kvp.Value.MaxCapacity} spaces");
        _ui.WriteLine();

        _currentGarageName = Utilities.AskForDictKey(
            "Please enter a garage name: ", _garages, _ui);
        _currentGarage = _garages[_currentGarageName];
        _ui.Write($"Garage {_currentGarageName} selected. Press enter to continue.");
        _ = _ui.ReadInput();
    }

    public void ViewAllGarages()
    {
        _ui.Clear();
        _ui.WriteLine("View all garages\n");

        foreach (var kvp in _garages)
        {
            var garage = kvp.Value;
            _ui.WriteLine(
                $"{kvp.Key}: {garage.MaxCapacity} spaces, {garage.Count} occupied.");
            _ui.WriteLine($"  {garage.VehicleSummary()}");
        }

        _ui.WriteLine("\nPress enter to continue.");
        _ = _ui.ReadInput();
    }

    // Inner action of AddNewGarage loop
    private void AddNewGarage_Single()
    {
        _ui.Clear();
        _ui.WriteLine("Add new garage\n");

        _ui.WriteLine($"Current garage names: {string.Join(", ", _garages.Keys)}\n");

        int numSpaces = (int)Utilities.AskForPositiveInt(
            "Number of spaces for the garage: ", _ui)!;
        string name = (string)Utilities.AskForString(
            "A nickname for the garage: ", _ui)!;
        
        if (_garages.ContainsKey(name))
        {
            _ui.WriteLine($"The list of garages already has one named {name}.");
            _ui.WriteLine("Press enter to continue.");
            _ = _ui.ReadInput();
            return;
        }

        try
        {
            var garage = new GarageHandler(numSpaces);
            _garages.Add(name, garage);
            _ui.WriteLine($"\nAdded garage {name} with {numSpaces} spaces.");
        }
        catch (Exception ex)
        {
            // numSpaces >= 1 and name cannot be empty or an existing value
            // I have no idea how we could end up here except something involving
            // index or memory limits
            _ui.WriteLine($"\nCould not add this garage; got exception {ex}");
        }
    }

    public void AddNewGarage()
        => Utilities.Loop(AddNewGarage_Single, "\nAdd another garage (y/n)? ", _ui);

    public void ViewSelectedGarage()
    {
        _ui.Clear();
        _ui.WriteLine("View selected garage\n");

        _ui.WriteLine($"Garage {_currentGarageName}");
        _ui.WriteLine($"Max capacity: {_currentGarage.MaxCapacity}");
        _ui.WriteLine($"Currently occupied spaces: {_currentGarage.Count}");

        _ui.WriteLine("\nVehicles currently parked:");
        _ui.WriteLine(_currentGarage.ListAll());
        _ui.Write("Press enter to continue.");
        _ = _ui.ReadInput();
    }

    private void AddVehicleToGarage_Single()
    {
        _ui.Clear();
        _ui.WriteLine("Add vehicle to garage\n");

        if (!_currentGarage.HasSpace)
        {
            _ui.WriteLine(
                "This garage has no space. Use RemoveVehicleFromGarage first.");
            return;
        }
        IVehicleFactory vehicleFactory = _vehiclePlant.ChooseVehicleFactory(_ui);
        Vehicle vehicle = vehicleFactory.BuildVehicle(
            _ui, vehicleIDs: _currentGarage.ListVehicleIDs());
        if (!_currentGarage.TryAdd(vehicle))
            _ui.WriteLine(
                "An error occurred while trying to add the vehicle to the garage");
        else
            _ui.WriteLine(
                $"Vehicle with ID {vehicle.VehicleID} was parked in the garage.");
    }

    public void AddVehicleToGarage()
        => Utilities.Loop(
            AddVehicleToGarage_Single, "Add another vehicle (y/n)? ", _ui);

    private void RemoveVehicleFromGarage_Single()
    {
        _ui.Clear();
        _ui.WriteLine("Remove vehicle from garage\n");

        if (_currentGarage!.Count == 0)
        {
            _ui.WriteLine("The current garage is empty; cannot remove a vehicle.");
            return;
        }

        _ui.WriteLine("The garage currently has these vehicles:");
        _ui.WriteLine(_currentGarage.ListAll());

        VehicleID vehicleID = (VehicleID)Utilities.AskForVehicleID(
            "Please enter the ID of one of these vehicles: ", _ui)!;
        if (!_currentGarage.RemoveByID(vehicleID.Code))
            _ui.WriteLine(
                $"Failed to remove the vehicle with ID {vehicleID}; "
                + "are you sure it was parked here?");
        else
            _ui.WriteLine(
                $"Vehicle with ID {vehicleID} was removed from the garage.");
    }

    public void RemoveVehicleFromGarage()
        => Utilities.Loop(
            RemoveVehicleFromGarage_Single, "Remove another vehicle (y/n)? ", _ui);
}
