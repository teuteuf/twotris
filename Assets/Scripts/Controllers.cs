using System.Collections.Generic;
using UnityEngine;

public class Controllers : MonoBehaviour
{
    public float delayStartAutoRepeat = 0.3f;
    public float delayMoveAutoRepeat = 0.05f;

    public TextMesh activePlayerText;
    public List<AbstractController> activeControllers;
    
    private static readonly List<PlayerInput> AutoRepeatableInputs = new List<PlayerInput>
    {
        PlayerInput.Left,
        PlayerInput.Right
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

    private int _currentControllerIndex;
    
    private AbstractController CurrentController => activeControllers[_currentControllerIndex];

    private void Start()
    {
        _currentControllerIndex = 0;
        UpdateActivePlayerText();
    }

    public List<PlayerInput> GetInputs()
    {
        var inputs = new List<PlayerInput>();
        if (CurrentController.IsPressed(PlayerInput.SoftDrop))
        {
            inputs.Add(PlayerInput.SoftDrop);
        }
        
        if (CurrentController.WasPressedThisFrame(PlayerInput.Clockwise))
        {
            inputs.Add(PlayerInput.Clockwise);
        }

        foreach (var autoRepeatableInput in AutoRepeatableInputs)
        {
            if (CurrentController.WasPressedThisFrame(autoRepeatableInput))
            {
                inputs.Add(autoRepeatableInput);
                _holdingStart[autoRepeatableInput] = Time.time;
                _lastMove[autoRepeatableInput] = Time.time;
            }
            if (IsAutoRepeatActive(autoRepeatableInput))
            {
                inputs.Add(autoRepeatableInput);
                _lastMove[autoRepeatableInput] = Time.time;
            }
        }

        return inputs;
    }

    private bool IsAutoRepeatActive(PlayerInput playerInput)
    {
        var isHoldingKey =CurrentController.IsPressed(playerInput);
        var holdKeyForLongEnough = Time.time - _holdingStart[playerInput] > delayStartAutoRepeat;
        var delayLastMoveElapsed = Time.time - _lastMove[playerInput] > delayMoveAutoRepeat;
        
        return isHoldingKey && holdKeyForLongEnough && delayLastMoveElapsed;
    }

    public void SwitchPlayer()
    {
        _currentControllerIndex = (_currentControllerIndex + 1) % activeControllers.Count;
        UpdateActivePlayerText();
    }

    private void UpdateActivePlayerText()
    {
        activePlayerText.text = (_currentControllerIndex + 1).ToString();
    }
}