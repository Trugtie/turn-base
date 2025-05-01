using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class ShootAction : BaseAction
{
    private float _totalSpinAmount;
    private int _maxShootDistance = 7;

    private Unit _targetUnit;
    private bool _canShootBullet;

    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {

        public Unit TargetUnit;
        public Unit ShootingUnit;
    }

    private enum ShootingState
    {
        Aiming,
        Shooting,
        Cooloff,
    }

    private ShootingState _state;

    private float _stateTimer;

    private void Update()
    {
        if (!_isActive) return;

        _stateTimer -= Time.deltaTime;

        switch (_state)
        {
            case ShootingState.Aiming:
                Aiming();
                break;
            case ShootingState.Shooting:
                if (_canShootBullet)
                {
                    Shoot();
                    _canShootBullet = false;
                }
                break;
            case ShootingState.Cooloff:
                break;
        }

        if (!(_stateTimer <= 0)) return;

        NextState();

    }

    private void Aiming()
    {
        Vector3 targetUnitDirection = (_targetUnit.GetWorldPosition() - _unit.GetWorldPosition()).normalized;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, targetUnitDirection, rotateSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            TargetUnit = _targetUnit,
            ShootingUnit = _unit,
        });

        _targetUnit.TakeDamge();
    }

    private void NextState()
    {
        switch (_state)
        {
            case ShootingState.Aiming:
                _state = ShootingState.Shooting;
                float shootingStateTime = 0.1f;
                _stateTimer = shootingStateTime;
                break;
            case ShootingState.Shooting:
                _state = ShootingState.Cooloff;
                float coolOffStateTime = 0.5f;
                _stateTimer = coolOffStateTime;
                break;
            case ShootingState.Cooloff:
                ActionComplete();
                break;
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onCompleteAction)
    {
        ActionStart(onCompleteAction);

        _state = ShootingState.Aiming;
        float aimingStateTime = 1f;
        _stateTimer = aimingStateTime;

        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        _targetUnit = targetUnit;
        _canShootBullet = true;

    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidListGridPosition()
    {
        List<GridPosition> validListGridPosition = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
        {
            for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                int testDistance = Math.Abs(x) + Math.Abs(z);
                if (testDistance > _maxShootDistance) continue;

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (_unit.IsEnemy() == targetUnit.IsEnemy()) continue;

                validListGridPosition.Add(testGridPosition);
            }
        }

        return validListGridPosition;
    }

    public override int GetActionPointCost()
    {
        return 2;
    }
}
