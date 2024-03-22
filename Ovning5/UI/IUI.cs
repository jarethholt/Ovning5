using System.Text;

namespace Ovning5.UI;

public class Options
    : Dictionary<string, (string name, string description, Action action)> { }

public interface IUI
{
    void Print();

    void Print(string prompt);

    string? ReadInput();

    void DisplayOptions(Options options)
    {
        int maxNameLength = options.Values.Select(value => value.name.Length).Max();
        Print("Choose one of these options.");

        StringBuilder stringBuilder = new();
        string format = $"{{0}} : {{1,-{maxNameLength}}} : {{2}}";
        foreach (var kvp in options)
            stringBuilder.AppendLine(
                string.Format(format, kvp.Key, kvp.Value.name, kvp.Value.description));
        Print(stringBuilder.ToString());
        Print();
    }
}