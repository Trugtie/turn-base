using System;
using System.Collections.Generic;
using UnityEditor.Splines;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [SerializeField] private GridSystemVisualSingle _gridSystemVisualSinglePrefab;

    [SerializeField] private List<GridVisualTypeMaterial> _gridVisualTypeMaterials;

    private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;

    private List<GridPosition> _rangeGridPositionCache;

    public enum GridVisualType
    {
        White,
        Blue,
        Green,
        Red,
        RedSoft,
        Yellow,
    }

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType GridVisualType;
        public Material Material;
    }

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

        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        LevelGrid.Instance.OnAnyMoveGridPosition += LevelGrid_OnAnyGridPositionChange;
        HealthSystem.OnAnyDead += HealthSystem_OnAnyDead;

        UpdateGridVisual();
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedActionChange -= UnitActionSystem_OnSelectedActionChange;
        LevelGrid.Instance.OnAnyMoveGridPosition -= LevelGrid_OnAnyGridPositionChange;
        HealthSystem.OnAnyDead -= HealthSystem_OnAnyDead;
    }

    private void HealthSystem_OnAnyDead(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyGridPositionChange(object sender, System.EventArgs e)
    {
        UpdateGridVisual();
    }

    private void UnitActionSystem_OnSelectedActionChange(object sender, System.EventArgs e)
    {
        UpdateGridVisual();
    }

    public void UpdateGridVisual()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        if (selectedAction == null) return;

        HideAllGridPosition();

        GridVisualType gridVisualType;

        switch (selectedAction)
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                break;
            case GrenadeAction grenadeAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            case SwordAction swordAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), swordAction.GetMaxSwordDistance(), GridVisualType.RedSoft);
                break;
            case InteractAction interactAction:
                gridVisualType = GridVisualType.Green;
                break;
        }

        ShowGridPositionList(selectedAction.GetValidListGridPosition(), gridVisualType);
    }

    public void HideAllGridPosition()
    {
        foreach (GridSystemVisualSingle singleGrid in _gridSystemVisualSingleArray)
        {
            singleGrid.Hide();
        }
    }

    private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        if (_rangeGridPositionCache == null)
        {
            _rangeGridPositionCache = new List<GridPosition>();
        };

        _rangeGridPositionCache.Clear();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                _rangeGridPositionCache.Add(testGridPosition);
            }
        }

        ShowGridPositionList(_rangeGridPositionCache, gridVisualType);
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        if (_rangeGridPositionCache == null)
        {
            _rangeGridPositionCache = new List<GridPosition>();
        };

        _rangeGridPositionCache.Clear();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                int testDistance = Math.Abs(x) + Math.Abs(z);
                if (testDistance > range) continue;

                _rangeGridPositionCache.Add(testGridPosition);
            }
        }

        ShowGridPositionList(_rangeGridPositionCache, gridVisualType);
    }

    private void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            _gridSystemVisualSingleArray[gridPosition.X, gridPosition.Z].Show(GetMaterialFormGridVisualType(gridVisualType));
        }
    }


    private Material GetMaterialFormGridVisualType(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in _gridVisualTypeMaterials)
        {
            if (gridVisualTypeMaterial.GridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.Material;
            }
        }
        Debug.Log("Count not find materials in " + gridVisualType);
        return null;
    }
}
