using UnityEngine;

public class Grid : MonoBehaviour
{
    public float timeBetweenFall = 1.0f;

    private float _delayBeforeFall;
    private Tetromino _currentTetromino;
    
    private Spawner _spawner;

    private void Start()
    {
        _delayBeforeFall = timeBetweenFall;

        _spawner = GetComponent<Spawner>();
        
        SpawnTetromino();
    }

    private void Update()
    {
        _delayBeforeFall -= Time.deltaTime;
        
        if (_delayBeforeFall <= 0.0f)
        {
            MoveDown();
            _delayBeforeFall = timeBetweenFall;
        }
    }

    private void MoveDown()
    {
        _currentTetromino.transform.position += Vector3.down;
        
        if (ShouldFreezeCurrentTetromino())
        {
            FreezeCurrentTetromino();
            SpawnTetromino();
        }
    }

    private void SpawnTetromino()
    {
        _currentTetromino = _spawner.Spawn();
    }

    private bool ShouldFreezeCurrentTetromino()
    {
        return Mathf.RoundToInt(_currentTetromino.transform.position.y) == 0;
    }

    private void FreezeCurrentTetromino()
    {
        IntegrateBlocks();
        Destroy(_currentTetromino.gameObject);
    }

    private void IntegrateBlocks()
    {
        foreach (var block in _currentTetromino.GetBlocks())
        {
            block.transform.SetParent(transform);
        }
    }
}
