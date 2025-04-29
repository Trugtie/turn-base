using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private Animator _animator;
    [SerializeField] private int _maxMoveDistance = 1;

    private const string IS_WALKING = "IsWalking";

    private Vector3 _targetPosition;



    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponentInChildren<Animator>();
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
        _animator.SetBool(IS_WALKING, false);
        if (moveDistance < stopThresoldDistance)
        {
            _isActive = false;
            _onActionComplete();
            return;
        };

        float speed = 4f;
        moveDirection.y = 0;
        transform.position += speed * Time.deltaTime * moveDirection;

        _animator.SetBool(IS_WALKING, true);
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositions = GetValidListGridPosition();
        return validGridPositions.Contains(gridPosition);
    }

    public List<GridPosition> GetValidListGridPosition()
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

    public void Move(GridPosition targetPosition, Action onActionComplete)
    {
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
        _isActive = true;
        _onActionComplete = onActionComplete;
    }

    public override string GetActionName()
    {
        return "Move";
    }

}
