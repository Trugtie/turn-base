using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float _timer;

    private enum EnemyAIState
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }

    private EnemyAIState _state;

    private void Awake()
    {
        _state = EnemyAIState.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
    }

    private void TurnSystem_OnTurnChange(object sender, System.EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;
        _state = EnemyAIState.TakingTurn;
        _timer = 2f;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;

        switch (_state)
        {
            case EnemyAIState.WaitingForEnemyTurn:
                break;
            case EnemyAIState.TakingTurn:

                _timer -= Time.deltaTime;

                if (_timer <= 0)
                {
                    if (TryTakingEnemyAIAction(SetTakingTurn))
                    {
                        _state = EnemyAIState.Busy;
                    }
                    else
                    {
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;

            case EnemyAIState.Busy:
                break;
        }
    }

    private void SetTakingTurn()
    {
        _timer = 0.5f;
        _state = EnemyAIState.TakingTurn;
    }

    private bool TryTakingEnemyAIAction(Action onEnemyActionComplete)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnits())
        {
            if (TakingEnemyAction(enemyUnit, onEnemyActionComplete))
            {
                return true;
            }
        }

        return false;
    }
    private bool TakingEnemyAction(Unit enemyUnit, Action onEnemyActionComplete)
    {
        BaseAction bestBaseAction = null;
        EnemyAIAction bestEnemyAIAction = null;

        foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {
            if (!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
                continue;

            EnemyAIAction aiAction = baseAction.GetBestEnemyAIAction();

            if (aiAction == null)
                continue;

            if (bestEnemyAIAction == null || aiAction.ActionValue > bestEnemyAIAction.ActionValue)
            {
                bestEnemyAIAction = aiAction;
                bestBaseAction = baseAction;
            }
        }

        if (bestEnemyAIAction != null && enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.GridPosition, onEnemyActionComplete);
            return true;
        }

        return false;
    }

}
