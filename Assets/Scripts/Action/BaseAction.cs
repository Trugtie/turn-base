using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

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
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        _isActive = false;
        _onActionComplete();
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();
        List<GridPosition> validGridPositionList = GetValidListGridPosition();

        foreach (GridPosition gridPosition in validGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }

        if (!(enemyAIActionList.Count > 0)) return null;

        enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => (b.ActionValue - a.ActionValue));

        return enemyAIActionList[0];
    }

    protected abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
    public Unit GetUnit() => _unit;
}
