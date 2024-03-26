namespace Ovning5.UI;

// The IUI interface is based on Console, so the ConsoleUI is a direct map
internal class ConsoleUI : IUI
{
    public void WriteLine() => Console.WriteLine();
    
    public void WriteLine(string prompt) => Console.WriteLine(prompt);

    public void Write(string prompt) => Console.Write(prompt);

    public string? ReadInput() => Console.ReadLine();

    public void Clear() => Console.Clear();
}
