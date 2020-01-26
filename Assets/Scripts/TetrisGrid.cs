using UnityEngine;

public class TetrisGrid : MonoBehaviour
{
    public float timeBetweenFall = 1.0f;

    private float _delayBeforeFall;
    private Tetromino _currentTetromino;
    private Block[,] _blocks;
    
    private Spawner _spawner;

    private static readonly Vector2Int GridSize = new Vector2Int(10, 40);

    private void Start()
    {
        _spawner = GetComponent<Spawner>();
        
        _delayBeforeFall = timeBetweenFall;
        _blocks = new Block[GridSize.x, GridSize.y];
        
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
        foreach (var block in _currentTetromino.GetBlocks())
        {
            var blockPosition = Vector2Int.RoundToInt(block.transform.position);
            if (blockPosition.y == 0)
            {
                return true;
            }

            if (_blocks[blockPosition.x, blockPosition.y - 1] != null)
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

            _blocks[blockPosition.x, blockPosition.y] = block;
        }
    }
}
