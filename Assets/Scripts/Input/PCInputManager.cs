using UnityEngine;
using System.Collections;

public class PCInputManager : IInputCommands {

    public bool CursorPressedDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool CursorPressedUp()
    {
        return Input.GetMouseButtonUp(0);
    }

    public bool CursorClick()
    {
        return Input.GetMouseButton(0);
    }
}
