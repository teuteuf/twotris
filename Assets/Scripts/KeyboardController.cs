using System.Collections.Generic;
using UnityEngine.InputSystem;

public class KeyboardController : AbstractController
{
    private static readonly Dictionary<PlayerInput, Key> InputsToKeys = new Dictionary<PlayerInput, Key>
    {
        {PlayerInput.SoftDrop, Key.DownArrow},
        {PlayerInput.Clockwise, Key.UpArrow},
        {PlayerInput.Counterclockwise, Key.LeftCtrl},
        {PlayerInput.Left, Key.LeftArrow},
        {PlayerInput.Right, Key.RightArrow},
    };
    
    public override bool WasPressedThisFrame(PlayerInput playerInput)
    {
        return Keyboard.current[InputsToKeys[playerInput]].wasPressedThisFrame;
    }

    public override bool IsPressed(PlayerInput playerInput)
    {
        return Keyboard.current[InputsToKeys[playerInput]].isPressed;
    }

    public override string ToString()
    {
        return "KEYBOARD ";
    }
}