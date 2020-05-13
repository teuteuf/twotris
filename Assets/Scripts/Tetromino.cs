using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    private const float LockDelay = 0.5f;
    private const int LockMoveNumber = 15;
    
    public Transform rotationCenter;
    
    private Block[] _blocks;
    
    private bool _landed;
    private float _landingTime;
    private int _nbLandedMoves;

    private void Start()
    {
        _blocks = GetComponentsInChildren<Block>();
    }

    public IEnumerable<Block> GetBlocks()
    {
        return _blocks;
    }

    public void Move(Vector2Int move, GridCell[,] gridCells)
    {
        var moveOffset = new Vector3(move.x, move.y);
        transform.position += moveOffset;

        if (!IsPositionValid(gridCells))
        {
            transform.position -= moveOffset;
        }

        if (_landed && move.x != 0)
        {
            IncrementLandedMoves();
        }
    }

    private bool IsPositionValid(GridCell[,] gridCells)
    {
        foreach (var block in _blocks)
        {
            var coordinates = Vector2Int.FloorToInt(block.transform.position);

            var outOfGridX = coordinates.x < 0 || coordinates.x >= gridCells.GetLength(0);
            var outOfGridY = coordinates.y < 0;
            if (outOfGridX || outOfGridY)
            {
                return false;
            }
            
            if (gridCells[coordinates.x, coordinates.y].Filled)
            {
                return false;
            }
        }

        return true;
    }

    public void Rotate(float rotationDegree, GridCell[,] gridCells)
    {
        transform.RotateAround(rotationCenter.transform.position, Vector3.forward, rotationDegree);

        if (!IsPositionValid(gridCells))
        {
            transform.RotateAround(rotationCenter.transform.position, Vector3.forward, -rotationDegree);
        }
        
        if (_landed)
        {
            IncrementLandedMoves();
        }
    }

    private void IncrementLandedMoves()
    {
        _nbLandedMoves++;
        _landingTime = Time.time;
    }

    public bool ShouldLock(GridCell[,] gridCells)
    {
        var isOnSurface = IsOnSurface(gridCells);
        if (isOnSurface && !_landed)
        {
            _landingTime = Time.time;
            _nbLandedMoves = 0;
        }
        
        _landed = isOnSurface;

        return _landed && (Time.time - _landingTime > LockDelay || _nbLandedMoves >= LockMoveNumber);
    }

    private bool IsOnSurface(GridCell[,] gridCells)
    {
        foreach (var block in _blocks)
        {
            var blockPosition = Vector2Int.FloorToInt(block.transform.position);
            if (blockPosition.y == 0)
            {
                return true;
            }

            if (gridCells[blockPosition.x, blockPosition.y - 1].Filled)
            {
                return true;
            }
        }

        return false;
    }
}
