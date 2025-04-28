using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GridObject
{
    private GridSystem _gridSystem;
    private GridPosition _gridPosition;

    private List<Unit> _listUnit;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
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
}
