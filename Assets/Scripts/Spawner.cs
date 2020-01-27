using UnityEngine;

public class Spawner : MonoBehaviour
{
    private static readonly Vector3 DefaultSpawnPosition = new Vector3(3, 20);

    public Tetromino[] tetrominoesPrefabs;
    
    public Tetromino Spawn()
    {
        var tetromino = Instantiate(PickRandomTetromino(), transform);
        tetromino.transform.position = DefaultSpawnPosition;
        
        return tetromino;
    }

    private Tetromino PickRandomTetromino()
    {
        return tetrominoesPrefabs[Random.Range(0, tetrominoesPrefabs.Length)];
    }
}
