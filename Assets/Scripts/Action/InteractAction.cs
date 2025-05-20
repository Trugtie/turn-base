using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class InteractAction : BaseAction
{
    private int _maxInteractDistance = 1;

    private void Update()
    {
        if (!_isActive)
            return;
    }

    public override string GetActionName()
    {
        return "Interact";
    }

    public override List<GridPosition> GetValidListGridPosition()
    {
        List<GridPosition> validListGridPosition = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxInteractDistance; x <= _maxInteractDistance; x++)
        {
            for (int z = -_maxInteractDistance; z <= _maxInteractDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                IInteractable interactableObject = LevelGrid.Instance.GetInteractableObjectAtGridPosition(testGridPosition);
                if (interactableObject == null) continue;

                validListGridPosition.Add(testGridPosition);
            }
        }

        return validListGridPosition;
    }

    public override void TakeAction(GridPosition gridPosition, Action onCompleteAction)
    {
        IInteractable interactableObject = LevelGrid.Instance.GetInteractableObjectAtGridPosition(gridPosition);
        interactableObject.Interact(OnInteractComplete);
        ActionStart(onCompleteAction);
    }

    protected override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            ActionValue = 0,
        };
    }

    private void OnInteractComplete()
    {
        ActionComplete();
    }
}
