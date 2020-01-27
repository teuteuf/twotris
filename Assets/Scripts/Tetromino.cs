using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    private Block[] _blocks;

    private void Start()
    {
        _blocks = GetComponentsInChildren<Block>();
    }

    public IEnumerable<Block> GetBlocks()
    {
        return _blocks;
    }

    public void Move(Vector2Int move)
    {
        transform.position += new Vector3(move.x, move.y);
    }
}
