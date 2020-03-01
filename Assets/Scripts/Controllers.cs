using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

public class Controllers : MonoBehaviour
{
    public float delayStartAutoRepeat = 0.3f;
    public float delayMoveAutoRepeat = 0.05f;
    
    private static readonly Dictionary<PlayerInput, GamepadButton> AutoRepeatableInputs = new Dictionary<PlayerInput, GamepadButton>
    {
        {PlayerInput.Left, GamepadButton.DpadLeft},
        {PlayerInput.Right, GamepadButton.DpadRight}
    };
    
    private readonly Dictionary<PlayerInput, float> _holdingStart = new Dictionary<PlayerInput, float>
    {
        {PlayerInput.Left, 0.0f},
        {PlayerInput.Right, 0.0f}
    };
    
    private readonly Dictionary<PlayerInput, float> _lastMove = new Dictionary<PlayerInput, float>
    {
        {PlayerInput.Left, 0.0f},
        {PlayerInput.Right, 0.0f}
    };

    private int _currentGamePadIndex;
    
    private Gamepad CurrentGamePad => Gamepad.all[_currentGamePadIndex];

    private void Start()
    {
        _currentGamePadIndex = 0;
    }

    public List<PlayerInput> GetInputs()
    {
        var inputs = new List<PlayerInput>();
        if (CurrentGamePad.dpad.down.isPressed)
        {
            inputs.Add(PlayerInput.SoftDrop);
        }
        
        if (CurrentGamePad.crossButton.wasPressedThisFrame)
        {
            inputs.Add(PlayerInput.Clockwise);
        }

        foreach (var autoRepeatableInput in AutoRepeatableInputs)
        {
            if (CurrentGamePad[autoRepeatableInput.Value].wasPressedThisFrame)
            {
                inputs.Add(autoRepeatableInput.Key);
                _holdingStart[autoRepeatableInput.Key] = Time.time;
                _lastMove[autoRepeatableInput.Key] = Time.time;
            }
            if (IsAutoRepeatActive(autoRepeatableInput.Value, autoRepeatableInput.Key))
            {
                inputs.Add(autoRepeatableInput.Key);
                _lastMove[autoRepeatableInput.Key] = Time.time;
            }
        }

        return inputs;
    }

    private bool IsAutoRepeatActive(GamepadButton button, PlayerInput playerInput)
    {
        var isHoldingKey = CurrentGamePad[button].isPressed;
        var holdKeyForLongEnough = Time.time - _holdingStart[playerInput] > delayStartAutoRepeat;
        var delayLastMoveElapsed = Time.time - _lastMove[playerInput] > delayMoveAutoRepeat;
        
        return isHoldingKey && holdKeyForLongEnough && delayLastMoveElapsed;
    }

    public void SwitchPlayer()
    {
        _currentGamePadIndex = (_currentGamePadIndex + 1) % Gamepad.all.Count;
    }
}