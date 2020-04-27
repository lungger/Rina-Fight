using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    static public bool IsPause = false;

    static public void PauseGame()
    {
        IsPause = true;
        Time.timeScale = 0;
        Debug.Log("pause game");
    }

    static public void ReStartGame()
    {
        IsPause = false;
        Time.timeScale = 1;
        Debug.Log("start game");
    }
}
