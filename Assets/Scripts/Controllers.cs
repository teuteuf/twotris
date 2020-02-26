using System.Collections.Generic;
using UnityEngine;

public class Controllers : MonoBehaviour
{
    public float delayStartAutoRepeat = 0.3f;
    public float delayMoveAutoRepeat = 0.05f;
    
    private static readonly Dictionary<PlayerInput, KeyCode> AutoRepeatableInputs = new Dictionary<PlayerInput, KeyCode>
    {
        {PlayerInput.Left, KeyCode.LeftArrow},
        {PlayerInput.Right, KeyCode.RightArrow}
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

    public List<PlayerInput> GetInputs()
    {
        var inputs = new List<PlayerInput>();
        if (Input.GetKey(KeyCode.DownArrow))
        {
            inputs.Add(PlayerInput.SoftDrop);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            inputs.Add(PlayerInput.Clockwise);
        }

        foreach (var autoRepeatableInput in AutoRepeatableInputs)
        {
            if (Input.GetKeyDown(autoRepeatableInput.Value))
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

    private bool IsAutoRepeatActive(KeyCode key, PlayerInput playerInput)
    {
        var isHoldingKey = Input.GetKey(key);
        var holdKeyForLongEnough = Time.time - _holdingStart[playerInput] > delayStartAutoRepeat;
        var delayLastMoveElapsed = Time.time - _lastMove[playerInput] > delayMoveAutoRepeat;
        
        return isHoldingKey && holdKeyForLongEnough && delayLastMoveElapsed;
    }
}