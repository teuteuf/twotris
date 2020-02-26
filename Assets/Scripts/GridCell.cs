using UnityEngine;

public class GridCell
{
    public bool Filled { get; private set; }
    
    private Block _block;

    public GridCell()
    {
        Filled = false;
        _block = null;
    }

    public void Fill(Block block)
    {
        Filled = true;
        _block = block;
    }

    public void Clear()
    {
        if (!Filled)
        {
            return;
        }
        
        _block.Destroy();
        Filled = false;
        _block = null;
    }

    public void MergeDown(GridCell upperGridCell)
    {
        if (!Filled && !upperGridCell.Filled)
        {
            return;
        }

        Filled = upperGridCell.Filled;
        _block = upperGridCell._block;
        upperGridCell.Filled = false;
        upperGridCell._block = null;
        
        if (Filled)
        {
            _block.transform.position += Vector3.down;
        }
    }
}