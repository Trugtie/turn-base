using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private TextMeshProUGUI _actionPointsText;
    [SerializeField] private Image _healthFillImage;
    [SerializeField] private Image _healthTempFillImage;
    [SerializeField] private float _delayFillTimerMax = 0.2f;

    private float _delayFillTimer;
    private float _previousHealthAmount;
    private float _targetHealthAmount;
    private bool _isHealthAnimating = false;

    private void Start()
    {
        Unit.OnAnyActionPointsChange += Unit_OnAnyActionPointsChange;
        _healthSystem.OnHealthChange += _healthSystem_OnHealthChange;

        UpdateActionPointsText();
        UpdateHealthVisual();
    }

    private void OnDestroy()
    {
        Unit.OnAnyActionPointsChange -= Unit_OnAnyActionPointsChange;
        _healthSystem.OnHealthChange -= _healthSystem_OnHealthChange;
    }

    private void Update()
    {
        HandleTempHealthFillAmount();
    }

    private void HandleTempHealthFillAmount()
    {
        if (!_isHealthAnimating) return;

        _delayFillTimer += Time.deltaTime;
        float t = Mathf.Clamp01(_delayFillTimer / _delayFillTimerMax);

        _healthTempFillImage.fillAmount = Mathf.Lerp(_previousHealthAmount, _targetHealthAmount, t);

        if (!(t >= 1f)) return;
        _isHealthAnimating = false;
    }

    private void _healthSystem_OnHealthChange(object sender, System.EventArgs e)
    {
        UpdateHealthVisual();
    }

    private void UpdateHealthVisual()
    {
        StartTempHealthAnimation();
        _healthFillImage.fillAmount = _targetHealthAmount;
    }

    private void StartTempHealthAnimation()
    {
        _isHealthAnimating = true;
        _delayFillTimer = 0f;
        _previousHealthAmount = _healthSystem.GetPreviousHealthAmount();
        _targetHealthAmount = _healthSystem.GetHealthNormalize();
    }

    private void Unit_OnAnyActionPointsChange(object sender, System.EventArgs e)
    {
        UpdateActionPointsText();
    }
    private void UpdateActionPointsText()
    {
        _actionPointsText.SetText(_unit.GetActionPoints().ToString());
    }
}
