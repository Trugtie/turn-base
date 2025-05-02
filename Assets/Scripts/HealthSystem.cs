using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 120;

    public event EventHandler OnDead;
    public event EventHandler OnHealthChange;

    private int _currentHealth;
    private float _previousHealthAmount;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void Damge(int healthAmount)
    {
        _previousHealthAmount = GetHealthNormalize();

        _currentHealth -= healthAmount;

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        OnHealthChange?.Invoke(this, EventArgs.Empty);

        if (_currentHealth == 0)
        {
            OnDead?.Invoke(this, EventArgs.Empty);
        }
    }

    public float GetHealthNormalize() => (float)_currentHealth / _maxHealth;

    public float GetPreviousHealthAmount() => _previousHealthAmount;
}
