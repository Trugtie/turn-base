using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private ActionButtonUI _actionButtonPrefab;
    [SerializeField] private Transform _actionButtonContainer;

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        CreateActionButtons();
    }

    private void UnitActionSystem_OnSelectedUnitChange(object sender, System.EventArgs e)
    {
        CreateActionButtons();
    }

    private void CreateActionButtons()
    {
        ClearAllActionButtons();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        if (selectedUnit == null) return;

        BaseAction[] baseActions = selectedUnit.GetBaseActionArray();
        foreach (BaseAction baseAction in baseActions)
        {
            ActionButtonUI actionButonUI = Instantiate(_actionButtonPrefab, _actionButtonContainer);
            actionButonUI.SetBaseAction(baseAction);
        }
    }

    private void ClearAllActionButtons()
    {
        foreach (Transform child in _actionButtonContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
