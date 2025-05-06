using System;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] private GrenadeProjectile _grenadeProjectilePrefab;
    private int _maxThrowDistance = 7;

    private void Update()
    {
        if (!_isActive) return;
    }

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override List<GridPosition> GetValidListGridPosition()
    {
        List<GridPosition> validListGridPosition = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxThrowDistance; x <= _maxThrowDistance; x++)
        {
            for (int z = -_maxThrowDistance; z <= _maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                int testDistance = Math.Abs(x) + Math.Abs(z);
                if (testDistance > _maxThrowDistance) continue;

                validListGridPosition.Add(testGridPosition);
            }
        }

        return validListGridPosition;
    }

    public override void TakeAction(GridPosition gridPosition, Action onCompleteAction)
    {
        Debug.Log("GrenadeAction Start");
        GrenadeProjectile grenadeInstance = Instantiate(_grenadeProjectilePrefab, transform.position, Quaternion.identity);
        grenadeInstance.Setup(gridPosition, OnGrenadeBehaviourComplete);
        ActionStart(onCompleteAction);
    }

    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
    }

    protected override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            ActionValue = 0,
        };
    }
}
