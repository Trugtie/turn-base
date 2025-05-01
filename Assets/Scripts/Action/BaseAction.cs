using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit _unit;
    protected bool _isActive;

    protected Action _onActionComplete;

    protected virtual void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onCompleteAction);

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositions = GetValidListGridPosition();
        return validGridPositions.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidListGridPosition();

    public virtual int GetActionPointCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        _isActive = true;
        _onActionComplete = onActionComplete;
    }

    protected void ActionComplete()
    {
        _isActive = false;
        _onActionComplete();
    }
}
