// Define format for passing UI menu options
global using Option = (string name, string description, System.Action action);
global using Options = System.Collections.Generic.List<
    (string name, string description, System.Action action)>;

using System.Text;

namespace Ovning5.UI;

internal interface IUI
{
    // Write a newline, write a string with a newline, or write a string
    void WriteLine();

    void WriteLine(string prompt);

    void Write(string prompt);

    // Read user input
    string? ReadInput();

    // Clear the current screen (before drawing the next menu)
    void Clear();

    // Implementation: display the different menu options
    void DisplayOptions(Options options, bool startAtZero = true)
    {
        WriteLine("Choose one of these options (by name or number).");

        StringBuilder stringBuilder = new();
        int maxNameLength = options.Select(option => option.name.Length).Max();
        // Format:   (index) : (function name) : (function description)
        // User can choose item based on function name or index
        string format = $"{{0}} : {{1,-{maxNameLength}}} : {{2}}";
        for (int i = 0; i < options.Count; i++)
        {
            var (name, description, action) = options[i];
            // The index should start at 0 if the first option is "Quit/Exit"
            // Otherwise more user-friendly to start at 1
            int index = startAtZero ? i : i + 1;
            stringBuilder.AppendLine(
                string.Format(format, index, name, description));
        }
        WriteLine(stringBuilder.ToString());
        WriteLine();
    }

}