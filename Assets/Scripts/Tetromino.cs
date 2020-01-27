using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public Transform rotationCenter;
    
    private Block[] _blocks;

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
    }

    private bool IsPositionValid(GridCell[,] gridCells)
    {
        foreach (var block in _blocks)
        {
            var coordinates = Vector2Int.RoundToInt(block.transform.position);

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
    }
}
