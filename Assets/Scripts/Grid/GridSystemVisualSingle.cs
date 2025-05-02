using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    private void Awake()
    {
        if (_meshRenderer != null) return;
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void Show(Material gridVisualMaterial)
    {
        _meshRenderer.material = gridVisualMaterial;
        _meshRenderer.enabled = true;
    }

    public void Hide()
    {
        _meshRenderer.enabled = false;
    }
}
