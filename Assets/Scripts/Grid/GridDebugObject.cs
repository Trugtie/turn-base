using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    private GridObject _gridObject;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshPro>();
    }

    public void SetGridObject(GridObject gridObject)
    {
        _gridObject = gridObject;
    }

    private void Update()
    {
        UpdateVisualData();
    }

    public void UpdateVisualData()
    {
        _text.SetText(_gridObject.ToString());
    }
}
