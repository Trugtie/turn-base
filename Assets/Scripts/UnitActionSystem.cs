using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChange;

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

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (TryHandleUnitSelection()) return;

        HandleSelectedAction();
    }

    private bool TryHandleUnitSelection()
    {
        if (!Input.GetMouseButtonDown(0)) return false;

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask)) return false;
        if (!raycastHit.transform.TryGetComponent<Unit>(out Unit unit)) return false;
        if (_selectedUnit == unit) return false;
        SetSelectedUnit(unit);
        return true;
    }

    private void HandleSelectedAction()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

        if (!_selectedAction.IsValidActionGridPosition(gridPosition)) return;

        SetBusy();
        _selectedAction.TakeAction(gridPosition, ClearBusy);
    }

    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
    }

    public void SetBusy()
    {
        _isBusy = true;
    }

    public void ClearBusy()
    {
        _isBusy = false;
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        _selectedAction = baseAction;
    }
    public Unit GetSelectedUnit() => _selectedUnit;

    public BaseAction GetSelectedAction() => _selectedAction;
}
