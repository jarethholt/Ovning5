﻿namespace Ovning5.UI;

public class ConsoleUI : IUI
{
    public void WriteLine() => Console.WriteLine();
    
    public void WriteLine(string prompt) => Console.WriteLine(prompt);

    public void Write(string prompt) => Console.Write(prompt);

    public string? ReadInput() => Console.ReadLine();
}
