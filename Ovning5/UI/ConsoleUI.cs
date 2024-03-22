namespace Ovning5.UI;

public class ConsoleUI : IUI
{
    public void Print(string prompt) => Console.WriteLine(prompt);

    public string? ReadInput() => Console.ReadLine();
}
