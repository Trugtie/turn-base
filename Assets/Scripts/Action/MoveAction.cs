using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private int _maxMoveDistance = 4;

    public event EventHandler OnMoveStart;
    public event EventHandler OnMoveEnd;

    private Vector3 _targetPosition;

    protected override void Awake()
    {
        base.Awake();
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (!_isActive) return;
        HandleMove();
    }

    private void HandleMove()
    {
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        float stopThresoldDistance = 0.1f;
        float moveDistance = Vector3.Distance(transform.position, _targetPosition);

        OnMoveEnd?.Invoke(this, EventArgs.Empty);

        if (moveDistance < stopThresoldDistance)
        {
            ActionComplete();
            return;
        };

        float speed = 4f;
        moveDirection.y = 0;
        transform.position += speed * Time.deltaTime * moveDirection;

        OnMoveStart?.Invoke(this, EventArgs.Empty);
    }

    public override List<GridPosition> GetValidListGridPosition()
    {
        List<GridPosition> validListGridPosition = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
        {
            for (int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (testGridPosition == unitGridPosition) continue;
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                validListGridPosition.Add(testGridPosition);
            }
        }

        return validListGridPosition;
    }

    public override void TakeAction(GridPosition targetPosition, Action onActionComplete)
    {
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
        ActionStart(onActionComplete);
    }

    public override string GetActionName()
    {
        return "Move";
    }

    protected override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int shootableUnitCount = _unit.GetAction<ShootAction>().GetShootableTargetCount(gridPosition);

        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            ActionValue = shootableUnitCount * 10,
        };
    }
}
