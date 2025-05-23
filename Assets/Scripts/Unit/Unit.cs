using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static event EventHandler OnAnyActionPointsChange;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [SerializeField] private bool _isEnemy;

    private const int MAX_ACTION_POINT = 2;

    private GridPosition _gridPosition;

    private BaseAction[] _baseActionArray;
    private HealthSystem _healthSystem;

    [SerializeField] private int _actionPoints = MAX_ACTION_POINT;

    private void Awake()
    {
        _baseActionArray = GetComponents<BaseAction>();
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        _healthSystem.OnDead += _healthSystem_OnDead;

        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void _healthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(_gridPosition, this);
        Destroy(gameObject);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition)
        {
            GridPosition oldGridPosition = _gridPosition;
            _gridPosition = newGridPosition;
            LevelGrid.Instance.MoveUnitToGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public override string ToString()
    {
        return $"Unit: {gameObject.name}";
    }


    private void TurnSystem_OnTurnChange(object sender, System.EventArgs e)
    {
        bool canResetActionPoint = IsEnemy() && TurnSystem.Instance.IsPlayerTurn()
                                || !IsEnemy() && !TurnSystem.Instance.IsPlayerTurn();

        if (canResetActionPoint) return;

        _actionPoints = MAX_ACTION_POINT;
        OnAnyActionPointsChange?.Invoke(this, EventArgs.Empty);
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (!CanSpendActionPointsToTakeAction(baseAction)) return false;

        SpendActionPoints(baseAction.GetActionPointCost());
        return true;
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return _actionPoints >= baseAction.GetActionPointCost();
    }

    public void SpendActionPoints(int amount)
    {
        _actionPoints -= amount;
        OnAnyActionPointsChange?.Invoke(this, EventArgs.Empty);
    }

    public void TakeDamge(int damgeAmount)
    {
        _healthSystem.Damge(damgeAmount);
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in _baseActionArray)
        {
            if (baseAction is T)
                return baseAction as T;
        }

        return null;
    }

    public Vector3 GetWorldPosition() => transform.position;

    public bool IsEnemy() => _isEnemy;

    public int GetActionPoints() => _actionPoints;

    public GridPosition GetGridPosition() => _gridPosition;

    public BaseAction[] GetBaseActionArray() => _baseActionArray;

    public float GetHealthNormalize() => _healthSystem.GetHealthNormalize();
}
