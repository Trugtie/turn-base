using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    private object _gridObject;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshPro>();
    }

    public virtual void SetGridObject(object gridObject)
    {
        _gridObject = gridObject;
    }

    private void Update()
    {
        UpdateVisualData();
    }

    protected virtual void UpdateVisualData()
    {
        _text.SetText(_gridObject.ToString());
    }
}
