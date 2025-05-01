using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private TextMeshProUGUI _turnText;
    [SerializeField] private GameObject _enemyTurnVisual;

    private void Start()
    {
        _endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;

        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButton();
    }

    private void TurnSystem_OnTurnChange(object sender, System.EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButton();
    }

    private void UpdateTurnText()
    {
        _turnText.SetText($"TURN {TurnSystem.Instance.GetCurrentTurnNumber()}");
    }

    private void UpdateEnemyTurnVisual()
    {
        _enemyTurnVisual.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButton()
    {
        _endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
