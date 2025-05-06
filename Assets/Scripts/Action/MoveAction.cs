using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private int _maxMoveDistance = 4;

    public event EventHandler OnMoveStart;
    public event EventHandler OnMoveEnd;

    private List<Vector3> _targetPositionList;
    private int _currentIndex;

    private void Update()
    {
        if (!_isActive) return;
        HandleMove();
    }

    private void HandleMove()
    {
        Vector3 targetPosition = _targetPositionList[_currentIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        float stopThresoldDistance = 0.1f;
        float moveDistance = Vector3.Distance(transform.position, targetPosition);

        if (moveDistance > stopThresoldDistance)
        {
            float speed = 4f;
            moveDirection.y = 0;
            transform.position += speed * Time.deltaTime * moveDirection;
        }
        else
        {
            _currentIndex++;

            if (_currentIndex >= _targetPositionList.Count)
            {
                OnMoveEnd?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }

        }
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
                if (!PathFinding.Instance.IsWalkableGridPosition(testGridPosition)) continue;
                if (!PathFinding.Instance.HasPath(unitGridPosition, testGridPosition)) continue;

                int pathFindingPathLenghtMultiplier = 10;

                if (PathFinding.Instance.GetPathLenght(
                    unitGridPosition, testGridPosition)
                    > _maxMoveDistance * pathFindingPathLenghtMultiplier)
                    continue;

                validListGridPosition.Add(testGridPosition);
            }
        }

        return validListGridPosition;
    }

    public override void TakeAction(GridPosition targetPosition, Action onActionComplete)
    {
        _currentIndex = 0;

        _targetPositionList = new List<Vector3>();
        List<GridPosition> pathGridPositionList = PathFinding.Instance.FindPath(_unit.GetGridPosition(), targetPosition, out int pathLenght);

        foreach (GridPosition gridPosition in pathGridPositionList)
        {
            _targetPositionList.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));
        }

        OnMoveStart?.Invoke(this, EventArgs.Empty);
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
