using UnityEngine;
using UnityEngine.UI;

public class LayerGame : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private Text currentLevel;
    [SerializeField] private Text nbLines;

    private void Update()
    {
        currentLevel.text = "Current Level: " + levelManager.CurrentLevel;
        nbLines.text = "Nb Lines: " + levelManager.NbLinesCleared;
    }
}
