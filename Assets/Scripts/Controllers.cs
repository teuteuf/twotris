using System.Collections.Generic;
using UnityEngine;

public class Controllers : MonoBehaviour
{
    public List<PlayerInput> GetInputs()
    {
        var inputs = new List<PlayerInput>();
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            inputs.Add(PlayerInput.SoftDrop);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            inputs.Add(PlayerInput.Left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            inputs.Add(PlayerInput.Right);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            inputs.Add(PlayerInput.Clockwise);
        }

        return inputs;
    }
}