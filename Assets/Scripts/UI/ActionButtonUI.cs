using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _actionText;
    [SerializeField] private Button _actionButton;
    [SerializeField] private GameObject _selectedGameObject;

    private BaseAction _baseAction;

    private void Awake()
    {
        _actionText = GetComponentInChildren<TextMeshProUGUI>();
        _actionButton = GetComponent<Button>();
    }

    public void SetBaseAction(BaseAction baseAction)
    {
        _baseAction = baseAction;

        _actionText.SetText(baseAction.GetActionName().ToUpper());

        _actionButton.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    private void Reset()
    {
        _actionText = GetComponentInChildren<TextMeshProUGUI>();
        _actionButton = GetComponent<Button>();
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        _selectedGameObject.SetActive(selectedAction == _baseAction);
    }
}
