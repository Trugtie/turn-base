using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 120;

    public event EventHandler OnDead;

    private int _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void Damge(int healthAmount)
    {
        _currentHealth -= healthAmount;

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        if (_currentHealth == 0)
        {
            OnDead?.Invoke(this, EventArgs.Empty);
        }

        Debug.Log(_currentHealth);
    }
}
