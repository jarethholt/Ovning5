using Ovning5.UI;
using Ovning5.VehicleCollections;
using Ovning5.VehicleFactories;
using Ovning5.Vehicles;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

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
            ("FilterVehiclesInGarage", "Search the vehicles in the garage",
                FilterVehiclesInGarage),
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

        // List the garages
        _ui.WriteLine("The following garages are available:");
        foreach (var kvp in _garages)
            _ui.WriteLine($"  {kvp.Key}: {kvp.Value.MaxCapacity} spaces");
        _ui.WriteLine();

        // Get the name of a garage
        _currentGarageName = Utilities.AskForDictKey(
            "Please enter a garage name: ", _garages, _ui);
        _currentGarage = _garages[_currentGarageName];
        _ui.Write($"Garage {_currentGarageName} selected. Press enter to continue.");
        _ = _ui.ReadInput();
    }

    // Show all garages and their vehicle summaries
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

    // Print out full vehicle properties for the current garage
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

        // Choose a factory from _vehiclePlant to construct the new vehicle
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

        // Show possible vehicles to remove
        _ui.WriteLine("The garage currently has these vehicles:");
        _ui.WriteLine(_currentGarage.ListAll());

        // Ask for the ID of the vehicle to remove
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

    private List<Vehicle> FilterVehiclesInGarage_Single(IEnumerable<Vehicle> vehicles)
    {
        _ui.Clear();
        _ui.WriteLine("Filter the vehicles in the current garage\n");

        if (!vehicles.Any())
        {
            _ui.WriteLine(
                "The current vehicle list is empty; cannot filter any further.");
            return vehicles.ToList();
        }

        // Display the current list
        PrintVehicleList(vehicles);
        _ui.WriteLine();

        // Vehicles can be filtered by type or by property
        _ui.WriteLine("Vehicles can either be filtered by type or property value.");
        bool byType = Utilities.AskForYesNo("Filter by type (y/n)? ", _ui);
        if (byType)
            return FilterVehicles_ByType(vehicles);
        else
            return FilterVehicles_ByProp(vehicles);
    }

    // Filter vehicles by type (Car, Boat, etc.)
    private List<Vehicle> FilterVehicles_ByType(IEnumerable<Vehicle> vehicles)
    {
        _ui.Clear();
        string vehicleTypeName = _vehiclePlant.ChooseVehicleType(_ui);
        return vehicles
            .Where(vehicle => vehicle.GetType().Name == vehicleTypeName)
            .ToList();
    }

    /* Filter vehicles by property
     * That's what this function is supposed to do. Technically, it filters
     * them by constructor parameter instead! This code was adapted from the
     * construction code in VehicleFactory, which relies on the list of
     * parameters written explicitly for each Vehicle type.
     * 
     * This means that, for example, it is not possible to filter vehicles
     * based on the Property NumberOfWheels because that's not a constructor
     * parameter for any vehicle (each type sets its own NumberOfWheels).
     */
    private List<Vehicle> FilterVehicles_ByProp(IEnumerable<Vehicle> vehicles)
    {
        _ui.Clear();

        // Display all possible parameters
        Dictionary<string, (Type type, List<string> appliesTo)> parameters
            = _vehiclePlant.GetAllParameters();
        _ui.WriteLine(
            "These are the possible parameters and the "
            + "Vehicle classes they apply to:");
        foreach (var kvp in parameters)
        {
            string paramName = kvp.Key;
            Type type = kvp.Value.type;
            List<string> appliesTo = kvp.Value.appliesTo;
            _ui.WriteLine(
                $"- {paramName} ({type.Name}): {string.Join(", ", appliesTo)}");
        }
        _ui.WriteLine();

        // Need a different logic for getting the value of each parameter's Type
        string param = Utilities.AskForDictKey(
            "Select a parameter: ", parameters, _ui);
        Type paramType = parameters[param].type;
        if (paramType.Equals(typeof(VehicleID)))
        {
            string prompt =
                $"{param}: Enter a string with (case-insensitive) format "
                + $"{VehicleID.CodeFormat}: ";
            VehicleID value = (VehicleID)Utilities.AskForVehicleID(prompt, _ui)!;
            return vehicles
                .Where(vehicle => vehicle.VehicleID.Equals(value))
                .ToList();
        }
        else if (paramType.Equals(typeof(bool)))
        {
            string prompt = $"{param}: Enter [y]es or [n]o: ";
            bool value = Utilities.AskForYesNo(prompt, _ui);
            return vehicles
                .Where(vehicle => TryGetPropertyValue(vehicle, param, out bool result)
                                  && (result == value))
                .ToList();
        }
        else if (paramType.Equals(typeof(int)))
        {
            string prompt = $"{param}: Enter a positive integer: ";
            int value = (int)Utilities.AskForPositiveInt(prompt, _ui)!;
            return vehicles
                .Where(vehicle => TryGetPropertyValue(vehicle, param, out int result)
                                  && (result == value))
                .ToList();
        }
        else if (paramType.Equals(typeof(string)))
        {
            string prompt = $"{param}: Enter a string: ";
            string value = (string)Utilities.AskForString(prompt, _ui)!;
            return vehicles
                .Where(vehicle => TryGetPropertyValue(vehicle, param, out string? result)
                                  && (value.Equals(result)))
                .ToList();
        }
        else
            throw new ArgumentException(
                $"Parameter {param} has a type {paramType.Name} that was unhandled. "
                + "Expected one of: VehicleID, bool, int, or string");
    }

    public void FilterVehiclesInGarage()
    {
        bool again;
        // Start with a list of all vehicles in the garage
        List<Vehicle> vehicles = _currentGarage.Where(
            vehicle => vehicle is not null).ToList()!;

        do
        {
            vehicles = FilterVehiclesInGarage_Single(vehicles);

            _ui.WriteLine("\nCurrent vehicle list:");
            PrintVehicleList(vehicles);
            _ui.WriteLine();
            if (vehicles.Count == 0)
                again = false;
            else
                again = Utilities.AskForYesNo("Continue filtering (y/n)? ", _ui);
        } while (again);
        _ui.WriteLine();

        // Display the current list
        if (vehicles.Count == 0)
        {
            _ui.WriteLine("The final list of vehicles is empty.");
        }
        else
        {
            _ui.Clear();
            _ui.WriteLine("Final filtered list of vehicles:\n");
            PrintVehicleList(vehicles);
        }
        _ui.WriteLine();
        _ui.WriteLine("Press enter to continue.");
        _ = _ui.ReadInput();
    }

    // Helper function to print the current vehicle list while filtering
    private void PrintVehicleList(IEnumerable<Vehicle> vehicles)
    {
        foreach (var vehicle in vehicles)
            _ui.WriteLine(vehicle.ToString());
    }

    /* This code is based on the following:
     * https://code-maze.com/csharp-get-a-value-of-a-property-by-using-its-name/
     * I needed a way to ask *all* objects in a list of Vehicles about the value
     * of a property given as a string. No Vehicle class has all of the properties
     * so I needed a way to signify that.
     * 
     * With this function, a query to match the property `paramName`
     * with the value `value` is written as
     *     TryGetPropertyValue(vehicle, paramName, out T result)
     *     && (result == value)
     * If the `vehicle` doesn't have the parameter, TryGetPropertyValue is false
     * and evaluation stops. If it does have the parameter and TryGetPropertyValue
     * is true, then `result` is non-null and also has to match the requested
     * `value`.
     */
    private static bool TryGetPropertyValue<TType, TObj>(
        TObj obj,
        string propertyName,
        [MaybeNullWhen(false)] out TType? value)
    {
        value = default;

        if (obj is null)
            return false;

        PropertyInfo? propertyInfo = typeof(TObj).GetProperty(propertyName);
        if (propertyInfo is null)
            return false;

        object? propertyValue = propertyInfo.GetValue(obj);
        /* Here the original code also checks whether the result value
         * could legitimately be null. That doesn't apply to any of my
         * cases but it seems safer to leave it in.
         * NB: Nullable.GetUnderlyingType actually returns null if
         * the type is *not* nullable. Go figure.
         */
        if (
            propertyValue is null
            && Nullable.GetUnderlyingType(typeof(TType)) is not null)
            return true;
        if (propertyValue is not TType typedValue)
            return false;

        value = typedValue;
        return true;
    }
}
