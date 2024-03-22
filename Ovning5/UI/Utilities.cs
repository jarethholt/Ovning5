using System.Diagnostics.CodeAnalysis;

namespace Ovning5.UI;

public delegate bool TryParse<T>(string input, [MaybeNullWhen(false)] out T result);

internal class Utilities(IUI ui)
{
    private readonly IUI _ui = ui;

    public void Loop(Action action, string againPrompt)
    {
        bool again;
        do
        {
            action();
            again = AskForYesNo(againPrompt);
        } while (again);
    }

    public T AskForBase<T>(string prompt, TryParse<T> tryParse, string errorFormatter)
    {
        string? readResult;
        T result;

        _ui.Write(prompt);
        do
        {
            readResult = _ui.ReadInput();
            if (string.IsNullOrWhiteSpace(readResult))
            {
                _ui.WriteLine("Cannot use an empty input. Try again.");
                continue;
            }

            if (!tryParse(readResult, out result!))
            {
                _ui.WriteLine(string.Format(errorFormatter, readResult));
                continue;
            }
            break;
        } while (true);
        return result;
    }

    public bool AskForYesNo(string prompt)
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
        return AskForBase<bool>(prompt, tryParse, errorFormatter);
    }

    public string AskForString(string prompt)
    {
        static bool tryParse(string readResult, out string result)
        {
            result = readResult;
            return true;
        }
        string errorFormatter = "";
        return AskForBase<string>(prompt, tryParse, errorFormatter);
    }

    public int AskForInt(string prompt)
    {
        string errorFormatter = "Could not parse '{0}' as an integer. Try again.";
        return AskForBase<int>(prompt, int.TryParse, errorFormatter);
    }

    public Action AskForOption(string prompt, Options options)
    {
        bool tryParse(string readResult, out string key)
        {
            key = readResult;
            return options.ContainsKey(key);
        }
        string keyList = string.Join(", ", options.Keys);
        string errorFormatter
            = $"Could not parse '{{0}}' as one of the valid options: {keyList}";
        string key = AskForBase<string>(prompt, tryParse, errorFormatter);
        return options[key].action;
    }
}
