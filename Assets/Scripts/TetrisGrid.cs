using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Spawner))]
[RequireComponent(typeof(Controllers))]
public class TetrisGrid : MonoBehaviour
{
    public float timeBetweenFall = 1.0f;

    private float _delayBeforeFall;
    private Tetromino _currentTetromino;
    private GridCell[,] _gridCells;

    private Spawner _spawner;
    private Controllers _controllers;

    private static readonly Vector2Int GridSize = new Vector2Int(10, 40);

    private static readonly Dictionary<PlayerInput, Vector2Int> InputsToMove = new Dictionary<PlayerInput, Vector2Int>
    {
        {PlayerInput.SoftDrop, Vector2Int.down},
        {PlayerInput.Left, Vector2Int.left},
        {PlayerInput.Right, Vector2Int.right}
    };
    
    private static readonly Dictionary<PlayerInput, float> InputsToRotate = new Dictionary<PlayerInput, float>
    {
        {PlayerInput.Clockwise, 90.0f},
        {PlayerInput.Counterclockwise, -90.0f}
    };

    private void Start()
    {
        _spawner = GetComponent<Spawner>();
        _controllers = GetComponent<Controllers>();

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
        UpdateTetrominoFreeze();
        UpdateTetrominoControl();
        UpdateTetrominoFall();
    }

    private void UpdateTetrominoFreeze()
    {
        if (ShouldFreezeCurrentTetromino())
        {
            FreezeCurrentTetromino();
            SpawnTetromino();
        }
    }

    private void UpdateTetrominoControl()
    {
        var playerInputs = _controllers.GetInputs();
        foreach (var playerInput in playerInputs)
        {
            if (InputsToMove.ContainsKey(playerInput))
            {
                _currentTetromino.Move(InputsToMove[playerInput], _gridCells);
            }
            
            if (InputsToRotate.ContainsKey(playerInput))
            {
                _currentTetromino.Rotate(InputsToRotate[playerInput], _gridCells);
            }
        }
    }

    private void UpdateTetrominoFall()
    {
        _delayBeforeFall -= Time.deltaTime;
        if (_delayBeforeFall <= 0.0f)
        {
            Fall();
            _delayBeforeFall += timeBetweenFall;
        }
    }

    private void Fall()
    {
        _currentTetromino.Move(Vector2Int.down, _gridCells);
    }

    private void SpawnTetromino()
    {
        _currentTetromino = _spawner.Spawn();
    }

    private bool ShouldFreezeCurrentTetromino()
    {
        foreach (var block in _currentTetromino.GetBlocks())
        {
            var blockPosition = Vector2Int.FloorToInt(block.transform.position);
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
            var blockPosition = Vector2Int.FloorToInt(blockTransform.position);
            
            blockTransform.SetParent(transform);

            _gridCells[blockPosition.x, blockPosition.y].Fill(block);
        }
    }
}
