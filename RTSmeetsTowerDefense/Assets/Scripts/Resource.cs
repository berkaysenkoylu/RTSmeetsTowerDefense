using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    // Class for every resource there will be in this game

    public string resourceName; // Variable for the name of the resource
    public int resourceYield; // Variable for yield of the resource (How many more player will have after having harvested the resource)

    public Texture2D cursorTexture;

    private void OnMouseOver()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
