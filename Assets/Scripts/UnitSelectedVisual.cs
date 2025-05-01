using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _unit = GetComponentInParent<Unit>();
        UpdateVisual();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange -= UnitActionSystem_OnSelectedUnitChange;

    }

    private void UnitActionSystem_OnSelectedUnitChange(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        bool isSelected = UnitActionSystem.Instance.GetSelectedUnit() == _unit;

        if (isSelected)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        _spriteRenderer.enabled = true;
    }

    private void Hide()
    {
        _spriteRenderer.enabled = false;
    }
}
