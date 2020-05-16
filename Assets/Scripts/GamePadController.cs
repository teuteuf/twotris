using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class GamePadController : AbstractController
{
    public int gamepadIndex;
    
    private static readonly Dictionary<PlayerInput, GamepadButton> InputsToButtons = new Dictionary<PlayerInput, GamepadButton>
    {
        {PlayerInput.SoftDrop, GamepadButton.DpadDown},
        {PlayerInput.Counterclockwise, GamepadButton.Cross},
        {PlayerInput.Clockwise, GamepadButton.Circle},
        {PlayerInput.Left, GamepadButton.DpadLeft},
        {PlayerInput.Right, GamepadButton.DpadRight},
    };
    
    public override bool WasPressedThisFrame(PlayerInput playerInput)
    {
        return Gamepad.all[gamepadIndex][InputsToButtons[playerInput]].wasPressedThisFrame;
    }

    public override bool IsPressed(PlayerInput playerInput)
    {
        return Gamepad.all[gamepadIndex][InputsToButtons[playerInput]].isPressed;
    }

    public override bool HasAnyKeyPressed()
    {
        return InputsToButtons.Values.Any(button => Gamepad.all[gamepadIndex][button].wasPressedThisFrame);
    }

    public override string ToString()
    {
        return "GAMEPAD " + gamepadIndex;
    }
}