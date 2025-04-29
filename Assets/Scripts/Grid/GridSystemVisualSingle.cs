using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        if (_spriteRenderer != null) return;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Show()
    {
        _spriteRenderer.enabled = true;
    }

    public void Hide()
    {
        _spriteRenderer.enabled = false;
    }
}
