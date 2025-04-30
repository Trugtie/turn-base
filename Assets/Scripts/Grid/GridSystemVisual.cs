using System.Collections.Generic;
using UnityEditor.Splines;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [SerializeField] private GridSystemVisualSingle _gridSystemVisualSinglePrefab;

    private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        int gridWidth = LevelGrid.Instance.GetWidth();
        int gridHeight = LevelGrid.Instance.GetHeight();

        _gridSystemVisualSingleArray = new GridSystemVisualSingle[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                GridSystemVisualSingle gridStstemVisualSingleInstance = Instantiate(_gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                _gridSystemVisualSingleArray[x, z] = gridStstemVisualSingleInstance;
            }
        }

        HideAllGridPosition();
    }

    private void Update()
    {
        UpdateGridVisual();
    }

    public void HideAllGridPosition()
    {
        foreach (GridSystemVisualSingle singleGrid in _gridSystemVisualSingleArray)
        {
            singleGrid.Hide();
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            _gridSystemVisualSingleArray[gridPosition.X, gridPosition.Z].Show();
        }
    }

    public void UpdateGridVisual()
    {
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        if (selectedAction == null) return;

        HideAllGridPosition();
        ShowGridPositionList(selectedAction.GetValidListGridPosition());
    }

}
