using Ovning5.Vehicles;
using System.Diagnostics.CodeAnalysis;

namespace Ovning5.UI;

public delegate bool TryParse<T>(string input, [MaybeNullWhen(false)] out T result);

internal static class Utilities
{
    public static void Loop(Action action, string againPrompt, IUI ui)
    {
        bool again;
        do
        {
            action();
            again = AskForYesNo(againPrompt, ui);
        } while (again);
    }

    public static T AskForBase<T>(
        string prompt,
        TryParse<T> tryParse,
        string errorFormatter,
        IUI ui)
    {
        string? readResult;
        T result;

        ui.Write(prompt);
        do
        {
            readResult = ui.ReadInput();
            if (string.IsNullOrWhiteSpace(readResult))
            {
                ui.WriteLine("Cannot use an empty input. Try again.");
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
        string errorFormatter = "Could not parse '{0}' as [y]es or [n]o. Try again.";
        return AskForBase<bool>(prompt, tryParse, errorFormatter, ui);
    }

    public static string AskForString(string prompt, IUI ui)
    {
        static bool tryParse(string readResult, out string result)
        {
            result = readResult;
            return true;
        }
        string errorFormatter = "";
        return AskForBase<string>(prompt, tryParse, errorFormatter, ui);
    }

    public static int AskForInt(string prompt, IUI ui)
    {
        string errorFormatter = $"Could not parse '{{0}}' as an integer. Try again.";
        return AskForBase<int>(prompt, int.TryParse, errorFormatter, ui);
    }

    public static int AskForInt(string prompt, int low, IUI ui)
    {
        bool tryParse(string readResult, out int result)
        {
            bool success = int.TryParse(readResult, out result);
            if (!success)
                return success;
            return (low <= result);
        }
        string errorFormatter
            = $"Could not parse '{{0}}' as an integer greater than {low-1}. Try again.";
        return AskForBase<int>(prompt, tryParse, errorFormatter, ui);
    }

    public static int AskForInt(string prompt, int low, int high, IUI ui)
    {
        bool tryParse(string readResult, out int result)
        {
            bool success = int.TryParse(readResult, out result);
            if (!success)
                return success;
            return (low <= result && result <= high);
        }
        string errorFormatter
            = $"Could not parse '{{0}}' as an integer between {low} and {high}. Try again.";
        return AskForBase<int>(prompt, tryParse, errorFormatter, ui);
    }

    public static int AskForPositiveInt(string prompt, IUI ui)
        => AskForInt(prompt, 1, ui);

    public static Action AskForOption(string prompt, Options options, IUI ui)
    {
        bool tryParse(string readResult, out string key)
        {
            key = readResult;
            return options.ContainsKey(key);
        }
        string keyList = string.Join(", ", options.Keys);
        string errorFormatter
            = $"Could not parse '{{0}}' as one of the valid options: {keyList}";
        string key = AskForBase<string>(prompt, tryParse, errorFormatter, ui);
        return options[key].action;
    }

    public static string AskForDictKey<TValue>(string prompt, Dictionary<string, TValue> dict, IUI ui)
    {
        bool tryParse(string readResult, out string key)
        {
            key = readResult;
            return dict.ContainsKey(key);
        }
        string keyList = string.Join(", ", dict.Keys);
        string errorFormatter = $"Could not parse '{{0}}' as one of the valid options: {keyList}";
        return AskForBase<string>(prompt, tryParse, errorFormatter, ui);
    }

    public static TValue AskForDictValue<TValue>(string prompt, Dictionary<string,TValue> dict, IUI ui)
    {
        string key = AskForDictKey<TValue>(prompt, dict, ui);
        return dict[key];
    }

    public static VehicleID AskForVehicleID(string prompt, IUI ui)
    {
        static bool tryParse(string readResult, out string vehicleID)
        {
            vehicleID = readResult;
            return VehicleID.Validate(vehicleID);
        }
        string errorFormatter
            = $"The input '{{0}}' must be a string of the form {VehicleID.CodeFormat}";
        return new VehicleID(AskForBase<string>(prompt, tryParse, errorFormatter, ui));
    }
}
