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
}