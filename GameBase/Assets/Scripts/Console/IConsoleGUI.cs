using System;

public interface IConsoleGUI
{
    void Init(Console console);
    void OnQuit(object sender, EventArgs args);
    void ConsoleUpdate();
    void SetOpen(bool open);
    bool Open();
    void Output(string message);
    void Clear(string clearMsg);
}