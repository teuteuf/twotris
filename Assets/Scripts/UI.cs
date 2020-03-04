using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField]
    private Text activeControllers;

    [SerializeField]
    private Controllers controllers;

    private void Update()
    {
        var result = "Active controllers:\n";
        foreach (var controller in controllers.activeControllers)
        {
            result += (controller + "\n");
        }

        result += "Press [Space]/A/(X) to join.\n";
        result += "Press again to start.\n";

        activeControllers.text = result;
    }
}
