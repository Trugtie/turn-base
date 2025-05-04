using System.Text;
using UnityEngine;

public class PathNode
{
    private GridPosition _gridPosition;

    private int _gCost;
    private int _hCost;
    private int _fCost;

    private PathNode _cameFormPathNote;

    public PathNode(GridPosition gridPosition)
    {
        _gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return $"{_gridPosition}";
    }

    public int GetGCost() => _gCost;
    public int GetHCost() => _hCost;
    public int GetFCost() => _fCost;
}
