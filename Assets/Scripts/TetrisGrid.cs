﻿using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Spawner))]
[RequireComponent(typeof(Controllers))]
public class TetrisGrid : MonoBehaviour
{
    public float timeBetweenFall = 1.0f;

    private bool _isSoftDropping;
    private float _timeLastFall;
    private Tetromino _currentTetromino;
    private GridCell[,] _gridCells;

    private Spawner _spawner;
    private Controllers _controllers;

    
    private static readonly Vector2Int GridSize = new Vector2Int(10, 40);

    private static readonly Dictionary<PlayerInput, Vector2Int> InputsToMove = new Dictionary<PlayerInput, Vector2Int>
    {
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

        _timeLastFall = Time.time;
        _isSoftDropping = false;
        InitGridCells();

        SpawnTetromino();
    }

    private void InitGridCells()
    {
        _gridCells = new GridCell[GridSize.x, GridSize.y];
        for (var x = 0; x < _gridCells.GetLength(0); x++)
        {
            for (var y = 0; y < _gridCells.GetLength(1); y++)
            {
                _gridCells[x, y] = new GridCell();
            }
        }
    }

    private void Update()
    {
        UpdateTetrominoControl();
        UpdateTetrominoFall();
        UpdateTetrominoFreeze();
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

        _isSoftDropping = playerInputs.Contains(PlayerInput.SoftDrop);
    }

    private void UpdateTetrominoFall()
    {
        var fallSpeedFactor = _isSoftDropping ? 20.0f : 1.0f;
        if (Time.time - _timeLastFall > timeBetweenFall / fallSpeedFactor)
        {
            Fall();
            _timeLastFall = Time.time;
        }
    }

    private void UpdateTetrominoFreeze()
    {
        if (ShouldFreezeCurrentTetromino())
        {
            FreezeCurrentTetromino();
            ClearFullLines();
            SpawnTetromino();
        }
    }

    private void Fall()
    {
        _currentTetromino.Move(Vector2Int.down, _gridCells);
    }

    private void SpawnTetromino()
    {
        _currentTetromino = _spawner.Spawn();
        _controllers.SwitchPlayer();
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

    private void ClearFullLines()
    {
        for (var y = _gridCells.GetLength(1) - 1; y >= 0; y--)
        {
            if (IsLineFull(y))
            {
                ClearLine(y);
            }
        }
    }

    private void ClearLine(int y)
    {
        for (var x = 0; x < _gridCells.GetLength(0); x++)
        {
            _gridCells[x, y].Clear();
            for (var mergingY = y; mergingY < _gridCells.GetLength(1) - 1; mergingY++)
            {
                _gridCells[x, mergingY].MergeDown(_gridCells[x, mergingY + 1]);
            }
        }
    }

    private bool IsLineFull(int y)
    {
        for (var x = 0; x < _gridCells.GetLength(0); x++)
        {
            if (!_gridCells[x, y].Filled)
            {
                return false;
            }
        }

        return true;
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
