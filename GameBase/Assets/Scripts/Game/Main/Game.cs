using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game GameObj { private set; get; }

    public readonly List<string> CommandArgs = new List<string>(Environment.GetCommandLineArgs());

    private Console Console;

    private void Awake()
    {
        if (GameObj != null)
            Destroy();

        DontDestroyOnLoad(this);

        Console = new Console(Instantiate(Resources.Load<GameObject>("Interface/Console")).GetComponent<IConsoleGUI>());
        Application.logMessageReceived += Console.OnDebugEntry;

        GameObj = this;
    }

    private void Update()
    {
        if (GameObj == null)
            return;

        Console.Update();
    }

    private void OnDestroy()
    {
        GameObj = null;
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
