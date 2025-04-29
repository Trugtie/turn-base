
using UnityEditor.Rendering;
using UnityEngine;

public class GridSystem
{
    private int _width;
    private int _height;
    private int _cellSize;

    private GridObject[,] _gridObjectArray;

    public GridSystem(int width, int height, int cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;

        _gridObjectArray = new GridObject[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                _gridObjectArray[x, z] = new GridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.X, 0, gridPosition.Z) * _cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / _cellSize),
            Mathf.RoundToInt(worldPosition.z / _cellSize)
            );
    }

    public void CreateGridDebugObjects(Transform gridDebugObjectPrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridDebugObjectTransform = GameObject.Instantiate(gridDebugObjectPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = gridDebugObjectTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return _gridObjectArray[gridPosition.X, gridPosition.Z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.X >= 0
            && gridPosition.Z >= 0
            && gridPosition.X < _width
            && gridPosition.Z < _height;
    }

    public int GetWidth() => _width;
    public int GetHeight() => _height;
}
