using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int NbLinesCleared { get; private set; }
    public int CurrentLevel { get; private set; }
    public float TimeBetweenFall { get; private set; }

    private void Start()
    {
        NbLinesCleared = 0;
        CurrentLevel = 0;
        TimeBetweenFall = ComputeTimeBetweenFall();
    }

    public void AddLineCleared()
    {
        NbLinesCleared++;
        CurrentLevel = Mathf.Min((NbLinesCleared / 10) + 1, 15);
        TimeBetweenFall = ComputeTimeBetweenFall();
    }

    private float ComputeTimeBetweenFall()
    {
        return Mathf.Pow(0.8f - ((CurrentLevel - 1) * 0.007f), CurrentLevel - 1);
    }
}
