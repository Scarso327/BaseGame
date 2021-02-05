using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameConsole : MonoBehaviour, IConsoleGUI
{
    public GameObject ConsoleParent;
    public InputField InputField;
    public TextMeshProUGUI Text;

    private Console Console;
    private Game Game;

    public void Init(Console console)
    {
        DontDestroyOnLoad(this); // Save me from scene loading...

        Game.OnGameQuit += OnQuit;

        Console = console;
        Game = FindObjectOfType<Game>();

        ConsoleParent.SetActive(false); // Inital Behaviour...
        Clear(""); // Ensure we're empty...
    }

    public void OnQuit(object sender, EventArgs args)
    {

    }

    public void ConsoleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            ConsoleParent.SetActive(!Open());

            if (Open())
                InputField.text = "";
        }

        if (Open())
        {
            InputField.ActivateInputField();

            if (Input.GetKeyDown(KeyCode.Return) && InputField.text.Length > 0)
            {
                Console.ExecCommand(InputField.text);
                InputField.text = "";
            }
        }
    }

    public void SetOpen(bool open)
    {
        ConsoleParent.SetActive(open);
    }

    public bool Open()
    {
        return ConsoleParent.activeInHierarchy;
    }

    public void Output(string message)
    {
        Text.text = $"{Text.text}\n{message}";
    }

    public void Clear(string clearMsg = "Console Cleared\n")
    {
        Text.text = clearMsg;
    }
}