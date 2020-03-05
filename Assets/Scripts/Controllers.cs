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
    
    private static readonly List<PlayerInput> SimpleInputs = new List<PlayerInput>
    {
        PlayerInput.Clockwise,
        PlayerInput.Counterclockwise
    };
    
    private static readonly List<PlayerInput> HoldingInputs = new List<PlayerInput>
    {
        PlayerInput.SoftDrop
    };
    
    private int _currentControllerIndex;
    private Dictionary<PlayerInput, float> _holdingStart;
    private Dictionary<PlayerInput, float> _lastMove;
    
    private AbstractController CurrentController => activeControllers[_currentControllerIndex];

    private void Start()
    {
        _currentControllerIndex = 0;
        
        _holdingStart = new Dictionary<PlayerInput, float>();
        _lastMove = new Dictionary<PlayerInput, float>();
        foreach (var autoRepeatableInput in AutoRepeatableInputs)
        {
            _holdingStart.Add(autoRepeatableInput, 0.0f);
            _lastMove.Add(autoRepeatableInput, 0.0f);
        }
        
        UpdateActivePlayerText();
    }

    public List<PlayerInput> GetInputs()
    {
        var inputs = new List<PlayerInput>();

        foreach (var holdingInput in HoldingInputs)
        {
            if (CurrentController.IsPressed(holdingInput))
            {
                inputs.Add(holdingInput);
            }
        }

        foreach (var simpleInput in SimpleInputs)
        {
            if (CurrentController.WasPressedThisFrame(simpleInput))
            {
                inputs.Add(simpleInput);
            }
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