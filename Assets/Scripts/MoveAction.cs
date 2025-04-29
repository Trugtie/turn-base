using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private int _maxMoveDistance = 1;

    private Unit _unit;

    private const string IS_WALKING = "IsWalking";

    private Vector3 _targetPosition;

    private void Awake()
    {
        _unit = GetComponent<Unit>();
        _animator = GetComponentInChildren<Animator>();
        _targetPosition = transform.position;
    }

    private void Update()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        float stopThresoldDistance = 0.1f;
        float moveDistance = Vector3.Distance(transform.position, _targetPosition);
        _animator.SetBool(IS_WALKING, false);
        if (moveDistance < stopThresoldDistance) return;

        float speed = 4f;
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        moveDirection.y = 0;
        transform.position += speed * Time.deltaTime * moveDirection;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

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

    public void Move(GridPosition targetPosition)
    {
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
    }


}
