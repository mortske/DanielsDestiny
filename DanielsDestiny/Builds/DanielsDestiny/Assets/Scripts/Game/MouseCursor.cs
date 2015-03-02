﻿using UnityEngine;
using System.Collections;

public class MouseCursor : MonoBehaviour {

    public Texture2D cursor;
    float cursorSize = 32;

    public void OnGUI()
    {
        if(PauseSystem.IsPaused)
            GUI.DrawTexture(new Rect(Input.mousePosition.x - cursorSize / 2 + cursorSize / 2, (Screen.height - Input.mousePosition.y) - cursorSize / 2 + cursorSize / 2, cursorSize, cursorSize), cursor);
    }
}
