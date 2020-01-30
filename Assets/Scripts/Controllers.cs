using System.Collections.Generic;
using UnityEngine;

public class Controllers : MonoBehaviour
{
    private static readonly Dictionary<PlayerInput, float> HoldingStart = new Dictionary<PlayerInput, float>
    {
        {PlayerInput.SoftDrop, 0.0f},
        {PlayerInput.Left, 0.0f},
        {PlayerInput.Right, 0.0f}
    };
    
    public List<PlayerInput> GetInputs()
    {
        var inputs = new List<PlayerInput>();
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            inputs.Add(PlayerInput.SoftDrop);
            HoldingStart[PlayerInput.SoftDrop] = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            inputs.Add(PlayerInput.Left);
            HoldingStart[PlayerInput.SoftDrop] = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            inputs.Add(PlayerInput.Right);
            HoldingStart[PlayerInput.SoftDrop] = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            inputs.Add(PlayerInput.Clockwise);
        }

        if (Input.GetKey(KeyCode.DownArrow) && Time.time - HoldingStart[PlayerInput.SoftDrop] > 0.5f)
        {
            inputs.Add(PlayerInput.SoftDrop);
        }

        return inputs;
    }
}