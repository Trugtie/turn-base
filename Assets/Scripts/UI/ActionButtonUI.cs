using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _actionText;
    [SerializeField] private Button _actionButton;

    private void Awake()
    {
        _actionText = GetComponentInChildren<TextMeshProUGUI>();
        _actionButton = GetComponent<Button>();
    }

    public void SetBaseAction(BaseAction baseAction)
    {
        _actionText.SetText(baseAction.GetActionName().ToUpper());
    }

    private void Reset()
    {
        _actionText = GetComponentInChildren<TextMeshProUGUI>();
        _actionButton = GetComponent<Button>();
    }
}
