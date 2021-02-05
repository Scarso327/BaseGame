using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console
{
    public delegate void MethodDelegate(string[] args);
    private static IConsoleGUI IConsoleGUI;

    private static readonly Dictionary<string, ConsoleCommand> AllCommands = new Dictionary<string, ConsoleCommand>();

    public Console(IConsoleGUI consoleGUI)
    {
        if (consoleGUI == null)
            return;

        AddCommand("help", "Prints all available commands to the console.", Help);

        IConsoleGUI = consoleGUI;
        IConsoleGUI.Init(this);
        IConsoleGUI.Output("Console Initialised");
    }

    public void Update()
    {
        IConsoleGUI.ConsoleUpdate();
    }

    public void ForceOpen(bool open)
    {
        IConsoleGUI.SetOpen(open);
    }

    public void OnDebugEntry(string logString, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Exception:
            case LogType.Error:
                logString = $"<#CC0000>{logString}<#FFFFFF>";
                break;
            case LogType.Warning:
                logString = $"<#E69138>{logString}<#FFFFFF>";
                break;
        }

        IConsoleGUI.Output(logString);
    }

    public static void AddCommand(string name, string description, MethodDelegate method)
    {
        name = name.ToLower();

        if (AllCommands.ContainsKey(name))
            return;

        AllCommands.Add(name, new ConsoleCommand(name, description, method));
    }

    public static bool RemoveCommand(string name)
    {
        if (!AllCommands.ContainsKey(name))
            return false;

        return AllCommands.Remove(name);
    }

    public static void ExecCommand(string input)
    {
        List<string> args = new List<string>(input.Split(null));

        // Get the actually command we want to execute...
        string command = args[0];
        args.RemoveAt(0);

        command = command.ToLower();

        if (AllCommands.TryGetValue(command, out ConsoleCommand consoleCommand))
        {
            IConsoleGUI.Output($"> {command}");
            consoleCommand.Method(args.ToArray());
        }
        else
        {
            IConsoleGUI.Output($"Unknown Command: {command}");
        }
    }

    private class ConsoleCommand
    {
        public string Name;
        public string Description;
        public MethodDelegate Method;

        public ConsoleCommand(string name, string description, MethodDelegate method)
        {
            this.Name = name;
            this.Description = description;
            this.Method = method;
        }
    }

    #region Console Commands

    private static void Help(string[] args)
    {
        IConsoleGUI.Output($"Available Commands:");

        foreach (ConsoleCommand command in AllCommands.Values)
            IConsoleGUI.Output($"  {command.Name}: {command.Description}");
    }

    #endregion
}