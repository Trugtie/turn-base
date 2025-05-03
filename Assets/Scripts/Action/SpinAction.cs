using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float _totalSpinAmount;

    private void Update()
    {
        if (!_isActive) return;

        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
        _totalSpinAmount += spinAddAmount;

        if (_totalSpinAmount >= 360f)
        {
            ActionComplete();
        }
    }
    public override void TakeAction(GridPosition gridPosition, Action onCompleteAction)
    {
        _totalSpinAmount = 0;
        ActionStart(onCompleteAction);
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidListGridPosition()
    {
        GridPosition gridPositon = _unit.GetGridPosition();

        return new List<GridPosition> { gridPositon };
    }

    public override int GetActionPointCost()
    {
        return 1;
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
