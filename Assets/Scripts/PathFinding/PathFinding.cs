using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] private Transform _gridDebugObject;

    private int _width;
    private int _height;
    private int _cellSize;

    private GridSystem<PathNode> _gridSystem;

    private void Awake()
    {
        _gridSystem = new GridSystem<PathNode>(10, 10, 2,
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        _gridSystem.CreateGridDebugObjects(_gridDebugObject);
    }
}
