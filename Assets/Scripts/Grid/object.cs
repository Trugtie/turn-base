using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GridObject
{
    private GridSystem<GridObject> _gridSystem;
    private GridPosition _gridPosition;

    private List<Unit> _listUnit;
    private IInteractable _interactableObject;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        _gridSystem = gridSystem;
        _gridPosition = gridPosition;
        _listUnit = new List<Unit>();
    }

    public override string ToString()
    {
        StringBuilder unitsString = new StringBuilder();

        foreach (Unit unit in _listUnit)
        {
            unitsString.Append(unit.ToString());
            unitsString.Append('\n');
        }

        return $"{_gridPosition} \n {unitsString}";
    }

    public List<Unit> GetListUnit()
    {
        return _listUnit;
    }

    public void AddUnit(Unit unit)
    {
        _listUnit.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        _listUnit.Remove(unit);
    }

    public bool HasAnyUnit()
    {
        return _listUnit.Count > 0;
    }

    public Unit GetUnit()
    {
        if (!HasAnyUnit()) return null;
        return _listUnit[0];
    }

    public IInteractable GetInteractableObject() => _interactableObject;
    public void SetInteractableObject(IInteractable interactableObject) { _interactableObject = interactableObject; }
}
