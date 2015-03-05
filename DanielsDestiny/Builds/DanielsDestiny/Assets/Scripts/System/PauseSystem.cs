using UnityEngine;
using System.Collections;

public class PauseSystem 
{
    public static bool IsPaused;

    public static void Pause(bool pause)
    {
        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

        //Screen.showCursor = pause;
        IsPaused = pause;
    }

    public static void TogglePause()
    {
        if (IsPaused)
            Pause(false);
        else
            Pause(true);
    }
}
