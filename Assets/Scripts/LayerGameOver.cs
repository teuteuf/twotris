using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LayerGameOver : MonoBehaviour
{

    [SerializeField] private Text score;
    [SerializeField] private Text restartText;
    [SerializeField] private Controllers controllers;

    private float _gameOverTime;

    public void DisplayGameOver(int nbLinesCleared)
    {
        gameObject.SetActive(true);
        score.text = nbLinesCleared.ToString();
    }

    private void Start()
    {
        _gameOverTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - _gameOverTime > 1.5f)
        {
            if (!restartText.IsActive())
            {
                restartText.gameObject.SetActive(true);
            }

            if (controllers.HasAnyKeyOrButtonPressedThisFrame())
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
