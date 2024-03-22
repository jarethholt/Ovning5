namespace Ovning5.UI;

public interface IUI
{
    void Print(string prompt);
    string? ReadInput();
}