using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static event EventHandler OnAnyActionPointsChange;

    [SerializeField] private bool _isEnemy;

    private const int MAX_ACTION_POINT = 2;

    private GridPosition _gridPosition;
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private BaseAction[] _baseActionArray;
    private HealthSystem _healthSystem;

    private int _actionPoints = MAX_ACTION_POINT;

    private void Awake()
    {
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _baseActionArray = GetComponents<BaseAction>();
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        _healthSystem.OnDead += _healthSystem_OnDead;

        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
    }

    private void _healthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(_gridPosition, this);
        Destroy(gameObject);
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

    public Vector3 GetWorldPosition() => transform.position;

    public bool IsEnemy() => _isEnemy;

    public int GetActionPoints() => _actionPoints;

    public MoveAction GetMoveAction() => _moveAction;

    public SpinAction GetSpinAction() => _spinAction;

    public GridPosition GetGridPosition() => _gridPosition;

    public BaseAction[] GetBaseActionArray() => _baseActionArray;
}
