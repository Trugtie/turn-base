using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private ActionButtonUI _actionButtonPrefab;
    [SerializeField] private Transform _actionButtonContainer;

    private List<ActionButtonUI> _listActionButtonUI;

    private void Awake()
    {
        _listActionButtonUI = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        CreateActionButtons();
        UpdateSelectedVisuals();
    }

    private void UnitActionSystem_OnSelectedActionChange(object sender, System.EventArgs e)
    {
        UpdateSelectedVisuals();
    }

    private void UnitActionSystem_OnSelectedUnitChange(object sender, System.EventArgs e)
    {
        CreateActionButtons();
        UpdateSelectedVisuals();
    }

    private void CreateActionButtons()
    {
        ClearAllActionButtons();

        _listActionButtonUI.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        if (selectedUnit == null) return;

        BaseAction[] baseActions = selectedUnit.GetBaseActionArray();
        foreach (BaseAction baseAction in baseActions)
        {
            ActionButtonUI actionButonUI = Instantiate(_actionButtonPrefab, _actionButtonContainer);
            actionButonUI.SetBaseAction(baseAction);
            _listActionButtonUI.Add(actionButonUI);
        }
    }

    private void ClearAllActionButtons()
    {
        foreach (Transform child in _actionButtonContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void UpdateSelectedVisuals()
    {
        foreach (ActionButtonUI actionButtonUI in _listActionButtonUI)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }
}
