using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : MonoBehaviour
{
    [SerializeField]
    private Controllers controllers;

    [SerializeField]
    private GameObject grid;
    
    private List<AbstractController> _activeControllers;

    private bool _keyboardActive;
    private List<int> _activeGamepadIndex;

    private void Start()
    {
        _keyboardActive = false;
        _activeGamepadIndex = new List<int>();
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (!_keyboardActive)
            {
                _keyboardActive = true;
                var keyboardController = new GameObject("KeyboardController").AddComponent<KeyboardController>();
                AddController(keyboardController);
            }
            else
            {
                StartGame();
            }
        }

        for (var gamepadIndex = 0; gamepadIndex < Gamepad.all.Count; gamepadIndex++)
        {
            var pressedCross = Gamepad.all[gamepadIndex].crossButton.wasPressedThisFrame;
            var notActive = !_activeGamepadIndex.Contains(gamepadIndex);
            if (pressedCross)
            {
                if (notActive)
                {
                    _activeGamepadIndex.Add(gamepadIndex);
                    var gamepadController = new GameObject("GamepadController_1").AddComponent<GamePadController>();
                    gamepadController.gamepadIndex = gamepadIndex;
                    AddController(gamepadController);
                }
                else
                {
                    StartGame();
                }
            }
        }
    }

    private void AddController(AbstractController controller)
    {
        controller.transform.parent = transform;
        controllers.activeControllers.Add(controller);
    }

    private void StartGame()
    {
        grid.SetActive(true);
        gameObject.SetActive(false);
    }
}
