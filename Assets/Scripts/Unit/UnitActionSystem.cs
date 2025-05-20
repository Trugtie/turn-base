using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChange;
    public event EventHandler OnSelectedActionChange;
    public event EventHandler OnBusyChange;
    public event EventHandler OnActionStarted;

    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private LayerMask _unitLayerMask;

    private BaseAction _selectedAction;
    private bool _isBusy;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Start()
    {
        if (_selectedUnit == null) return;

        SetSelectedUnit(_selectedUnit);
    }

    private void Update()
    {
        if (_isBusy) return;

        if (!TurnSystem.Instance.IsPlayerTurn()) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (TryHandleUnitSelection()) return;

        HandleSelectedAction();
    }

    private bool TryHandleUnitSelection()
    {
        if (!InputSystem.Instance.GetMouseButtonDown()) return false;

        Ray mouseRay = Camera.main.ScreenPointToRay(InputSystem.Instance.GetMousePosition());
        if (!Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask)) return false;
        if (!raycastHit.transform.TryGetComponent<Unit>(out Unit unit)) return false;
        if (_selectedUnit == unit) return false;
        if (unit.IsEnemy()) return false;
        SetSelectedUnit(unit);
        return true;
    }

    private void HandleSelectedAction()
    {
        if (!InputSystem.Instance.GetMouseButtonDown()) return;

        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

        if (!_selectedAction.IsValidActionGridPosition(gridPosition)) return;

        if (!_selectedUnit.TrySpendActionPointsToTakeAction(_selectedAction)) return;

        SetBusy();
        _selectedAction.TakeAction(gridPosition, ClearBusy);
        OnActionStarted?.Invoke(this, EventArgs.Empty);
    }

    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        SetSelectedAction(unit.GetAction<MoveAction>());
        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
    }

    public void SetBusy()
    {
        _isBusy = true;
        OnBusyChange?.Invoke(this, EventArgs.Empty);
    }

    public bool IsBusy()
    {
        return _isBusy;
    }

    public void ClearBusy()
    {
        _isBusy = false;
        OnBusyChange?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        _selectedAction = baseAction;
        OnSelectedActionChange?.Invoke(this, EventArgs.Empty);
    }
    public Unit GetSelectedUnit() => _selectedUnit;

    public BaseAction GetSelectedAction() => _selectedAction;
}
