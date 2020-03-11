using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LayerControllerSelection : MonoBehaviour
{
    [SerializeField] private Text activeControllers;
    [SerializeField] private LayerGame layerGame;
    
    [SerializeField] private Controllers controllers;
    [SerializeField] private TetrisGrid tetrisGrid;

    private void Update()
    {
        var result = controllers.activeControllers.Aggregate(
            "",
            (current, controller) => current + (controller + "\n")
        );

        activeControllers.text = result;

        if (tetrisGrid.isActiveAndEnabled)
        {
            gameObject.SetActive(false);
            layerGame.gameObject.SetActive(true);
        }
    }
}
