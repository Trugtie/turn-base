using System.Text;
using UnityEngine;

public class PathNode
{
    private GridPosition _gridPosition;

    private int _gCost;
    private int _hCost;
    private int _fCost;

    private PathNode _cameFromPathNote;

    private bool _isWalkable = true;

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

    public void SetGCost(int gCost)
    {
        _gCost = gCost;
    }

    public void SetHCost(int hCost)
    {
        _hCost = hCost;
    }

    public void CalculateFCost()
    {
        _fCost = _gCost + _hCost;
    }

    public void ResetCameFromPathNode()
    {
        _cameFromPathNote = null;
    }

    public void SetCameFromPathNode(PathNode pathNode)
    {
        _cameFromPathNote = pathNode;
    }

    public void SetWalkable(bool isWalkable)
    {
        _isWalkable = isWalkable;
    }

    public bool IsWalkable() => _isWalkable;
    public PathNode GetCameFromPathNode() => _cameFromPathNote;
    public GridPosition GetGridPosition() => _gridPosition;
}
