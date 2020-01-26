using UnityEngine;

public class TetrisGrid : MonoBehaviour
{
    public float timeBetweenFall = 1.0f;

    private float _delayBeforeFall;
    private Tetromino _currentTetromino;
    private GridCell[,] _gridCells;
    
    private Spawner _spawner;

    private static readonly Vector2Int GridSize = new Vector2Int(10, 40);

    private void Start()
    {
        _spawner = GetComponent<Spawner>();
        
        _delayBeforeFall = timeBetweenFall;
        InitGridCells();

        SpawnTetromino();
    }

    private void InitGridCells()
    {
        _gridCells = new GridCell[GridSize.x, GridSize.y];
        for (var i = 0; i < _gridCells.GetLength(0); i++)
        {
            for (var j = 0; j < _gridCells.GetLength(1); j++)
            {
                _gridCells[i, j] = new GridCell();
            }
        }
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
        foreach (var block in _currentTetromino.GetBlocks())
        {
            var blockPosition = Vector2Int.RoundToInt(block.transform.position);
            if (blockPosition.y == 0)
            {
                return true;
            }

            if (_gridCells[blockPosition.x, blockPosition.y - 1].Filled)
            {
                return true;
            }
        }

        return false;
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
            var blockTransform = block.transform;
            var blockPosition = Vector2Int.RoundToInt(blockTransform.position);
            
            blockTransform.SetParent(transform);

            _gridCells[blockPosition.x, blockPosition.y].Fill(block);
        }
    }
}
