using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChange;

    private int _currentTurnNumber = 1;

    private bool _isPlayerTurn = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public void NextTurn()
    {
        _currentTurnNumber++;

        _isPlayerTurn = !_isPlayerTurn;

        OnTurnChange?.Invoke(this, EventArgs.Empty);
    }

    public int GetCurrentTurnNumber() => _currentTurnNumber;

    public bool IsPlayerTurn() => _isPlayerTurn;
}
