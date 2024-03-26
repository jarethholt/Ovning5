using Ovning5.Vehicles;
using System.Diagnostics.CodeAnalysis;

namespace Ovning5.UI;

// Generic format for a TryParse function
public delegate bool TryParse<T>(string input, [MaybeNullWhen(false)] out T result);

internal static class Utilities
{
    // Loop an action, asking the user if they want to do it again each time
    public static void Loop(Action action, string againPrompt, IUI ui)
    {
        bool again;
        do
        {
            action();
            again = AskForYesNo(againPrompt, ui);
        } while (again);
    }

    /* Base generic method for asking for user input.
     * First checks if a non-empty input is given.
     * Then uses a TryParse-like function to see if the input is valid.
     * If invalid, errorFormatter is used to describe what didn't work.
     * 
     * If empty input leads to a default value, set T to be nullable
     * and this function will return null.
     */
    public static T? AskForBase<T>(
        string prompt,
        TryParse<T> tryParse,
        string errorFormatter,
        IUI ui,
        bool isEmptyOk = false)
    {
        string? readResult;
        T? result = default;

        do
        {
            ui.Write(prompt);
            readResult = ui.ReadInput();
            if (string.IsNullOrWhiteSpace(readResult))
            {
                if (isEmptyOk)
                    return result;
                ui.WriteLine("Cannot use an empty input.");
                continue;
            }

            if (!tryParse(readResult, out result!))
            {
                ui.WriteLine(string.Format(errorFormatter, readResult));
                continue;
            }
            break;
        } while (true);
        return result;
    }

    // Ask a yes/no question
    public static bool AskForYesNo(string prompt, IUI ui)
    {
        static bool tryParse(string readResult, out bool result)
        {
            bool success = false;
            string answerString = readResult.Trim().ToLower();
            if (answerString.StartsWith('y'))
            {
                result = true;
                success = true;
            }
            else if (answerString.StartsWith('n'))
            {
                result = false;
                success = true;
            }
            else result = default;
            return success;
        }
        string errorFormatter = "Could not parse '{0}' as [y]es or [n]o.";
        return AskForBase<bool>(prompt, tryParse, errorFormatter, ui);
    }

    // Ask for simple string input (no validation)
    public static string? AskForString(string prompt, IUI ui, bool isEmptyOk = false)
    {
        static bool tryParse(string readResult, out string result)
        {
            result = readResult;
            return true;
        }
        string errorFormatter = "";
        return AskForBase<string>(prompt, tryParse, errorFormatter, ui, isEmptyOk);
    }

    // Ask for an integer (no constraint)
    public static int AskForInt(string prompt, IUI ui, bool isEmptyOk = false)
    {
        string errorFormatter = $"Could not parse '{{0}}' as an integer.";
        return AskForBase<int>(prompt, int.TryParse, errorFormatter, ui, isEmptyOk);
    }

    // Ask for an integer with a lower bound
    public static int AskForInt(string prompt, int low, IUI ui, bool isEmptyOk = false)
    {
        bool tryParse(string readResult, out int result)
        {
            bool success = int.TryParse(readResult, out result);
            if (!success)
                return success;
            return (low <= result);
        }
        string errorFormatter
            = $"Could not parse '{{0}}' as an integer greater than {low-1}.";
        return AskForBase<int>(prompt, tryParse, errorFormatter, ui, isEmptyOk);
    }

    // Ask for an integer with lower and upper bounds
    public static int AskForInt(
        string prompt,
        int low,
        int high,
        IUI ui,
        bool isEmptyOk = false)
    {
        bool tryParse(string readResult, out int result)
        {
            bool success = int.TryParse(readResult, out result);
            if (!success)
                return success;
            return (low <= result && result <= high);
        }
        string errorFormatter
            = $"Could not parse '{{0}}' as an integer between {low} and {high}.";
        return AskForBase<int>(prompt, tryParse, errorFormatter, ui, isEmptyOk);
    }

    // Ask for an integer > 0 (>= 1)
    public static int AskForPositiveInt(string prompt, IUI ui, bool isEmptyOk = false)
        => AskForInt(prompt, 1, ui, isEmptyOk);

    // Ask for a menu option
    // Menu options can be given either by their index or their name
    // Returns the Action specified by the menu
    public static Action AskForOption(
        string prompt,
        Options options,
        IUI ui,
        bool startAtZero = true)
    {
        bool tryParse(string readResult, [MaybeNullWhen(false)] out Option option)
        {
            option = default;
            if (int.TryParse(readResult, out int index))
            {
                int i = startAtZero ? index : index - 1;
                if (0 <= i && i < options.Count)
                {
                    option = options[index];
                    return true;
                }
                else
                    return false;
            }
            Option? result = options.Find(
                option => option.name.Equals(
                    readResult, StringComparison.CurrentCultureIgnoreCase));
            if (result is (null, null, null))
                return false;
            option = (Option)result;
            return true;
        }

        string errorFormatter
            = $"Could not parse '{{0}}' as one of the valid options.";
        Option option = AskForBase<Option>(prompt, tryParse, errorFormatter, ui);
        return option.action;
    }

    // Ask for a key from a (string, T) dictionary
    public static string AskForDictKey<TValue>(
        string prompt,
        Dictionary<string, TValue> dict,
        IUI ui)
    {
        bool tryParse(string readResult, out string key)
        {
            key = readResult;
            return dict.ContainsKey(key);
        }
        string keyList = string.Join(", ", dict.Keys);
        string errorFormatter = $"Could not parse '{{0}}' as one of the valid options: {keyList}";
        return AskForBase<string>(prompt, tryParse, errorFormatter, ui)!;
    }

    // Ask for a value from a (string, T) dictionary
    public static TValue AskForDictValue<TValue>(
        string prompt,
        Dictionary<string, TValue> dict,
        IUI ui)
    {
        string key = AskForDictKey<TValue>(prompt, dict, ui);
        return dict[key];
    }

    // Ask for a VehicleID, which must match in format
    // Inputs are converted to uppercase first
    // An empty/default value could be used to generate a random VehicleID
    public static VehicleID? AskForVehicleID(
        string prompt,
        IUI ui,
        bool isEmptyOk = false)
    {
        static bool tryParse(string readResult, out string vehicleID)
        {
            vehicleID = readResult.ToUpper();
            return VehicleID.Validate(vehicleID);
        }
        string errorFormatter
            = $"The input '{{0}}' must be a string of the form {VehicleID.CodeFormat}";
        string? code = AskForBase<string>(
            prompt, tryParse,errorFormatter,ui,isEmptyOk);
        return code is null ? null : new VehicleID(code);
    }
}
