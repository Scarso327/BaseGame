using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static bool IsReady = false;

    public readonly List<string> CommandArgs = new List<string>(Environment.GetCommandLineArgs());

    private Console Console;

    private void Awake()
    {
        if (IsReady)
            Destroy();

        DontDestroyOnLoad(this);

        Console = new Console(Instantiate(Resources.Load<GameObject>("Interface/Console")).GetComponent<IConsoleGUI>());

        IsReady = true;
    }

    private void Update()
    {
        if (!IsReady)
            return;

        Console.Update();
    }

    private void OnDestroy()
    {
        IsReady = false;
    }

    private void Destroy()
    {
        Destroy(this);
    }

    public static event EventHandler OnGameQuit;

    private void OnApplicationQuit()
    {
        OnGameQuit.Invoke(this, EventArgs.Empty);
    }
}