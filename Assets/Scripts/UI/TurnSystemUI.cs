using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private TextMeshProUGUI _turnText;

    private void Start()
    {
        _endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;

        UpdateTurnText();
    }

    private void TurnSystem_OnTurnChange(object sender, System.EventArgs e)
    {
        UpdateTurnText();
    }

    private void UpdateTurnText()
    {
        _turnText.SetText($"TURN {TurnSystem.Instance.GetCurrentTurnNumber()}");
    }
}
