using System;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    public static event EventHandler OnAnySwordHit;

    public event EventHandler OnSwordActionStart;
    public event EventHandler OnSwordActionEnd;

    private int _maxSwordDistance = 1;
    private Unit _targetUnit;

    private enum SwordState
    {
        SwingingSwordBerforeHit,
        SwingingSwordAfterHit,
    }

    private SwordState _state;

    private float _stateTimer;

    private void Update()
    {
        if (!_isActive) return;

        _stateTimer -= Time.deltaTime;

        switch (_state)
        {
            case SwordState.SwingingSwordBerforeHit:
                Aiming();
                break;
            case SwordState.SwingingSwordAfterHit:
                break;
        }

        if (!(_stateTimer <= 0)) return;

        NextState();
    }

    private void NextState()
    {
        switch (_state)
        {
            case SwordState.SwingingSwordBerforeHit:
                _targetUnit.TakeDamge(100);
                _state = SwordState.SwingingSwordAfterHit;
                float afterHitStateTime = 0.5f;
                _stateTimer = afterHitStateTime;
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                break;
            case SwordState.SwingingSwordAfterHit:
                OnSwordActionEnd?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    private void Aiming()
    {
        Vector3 targetUnitDirection = (_targetUnit.GetWorldPosition() - _unit.GetWorldPosition()).normalized;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, targetUnitDirection, rotateSpeed * Time.deltaTime);
    }

    public override string GetActionName()
    {
        return "Sword";
    }

    public override List<GridPosition> GetValidListGridPosition()
    {
        List<GridPosition> validListGridPosition = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxSwordDistance; x <= _maxSwordDistance; x++)
        {
            for (int z = -_maxSwordDistance; z <= _maxSwordDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (_unit.IsEnemy() == targetUnit.IsEnemy()) continue;

                validListGridPosition.Add(testGridPosition);
            }
        }

        return validListGridPosition;
    }

    public override void TakeAction(GridPosition gridPosition, Action onCompleteAction)
    {
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        _state = SwordState.SwingingSwordBerforeHit;
        float beforeHitStateTime = 0.7f;
        _stateTimer = beforeHitStateTime;
        ActionStart(onCompleteAction);
        OnSwordActionStart?.Invoke(this, EventArgs.Empty);
    }

    public int GetMaxSwordDistance() => _maxSwordDistance;

    protected override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            ActionValue = 200,
        };
    }
}
