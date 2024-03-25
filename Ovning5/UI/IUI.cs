global using Option = (string name, string description, System.Action action);
global using Options = System.Collections.Generic.List<
    (string name, string description, System.Action action)>;

using System.Text;

namespace Ovning5.UI;

internal interface IUI
{
    void WriteLine();

    void WriteLine(string prompt);

    void Write(string prompt);

    string? ReadInput();
    void Clear();

    void DisplayOptions(Options options, bool startAtZero = true)
    {
        int maxNameLength = options.Select(option => option.name.Length).Max();
        WriteLine("Choose one of these options (by name or number).");

        StringBuilder stringBuilder = new();
        string format = $"{{0}} : {{1,-{maxNameLength}}} : {{2}}";
        for (int i = 0; i < options.Count; i++)
        {
            var (name, description, action) = options[i];
            int index = startAtZero ? i : i + 1;
            stringBuilder.AppendLine(
                string.Format(format, index, name, description));
        }
        WriteLine(stringBuilder.ToString());
        WriteLine();
    }

}