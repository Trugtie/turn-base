using TMPro;
using UnityEngine;

public class PathFindingGridDebugObject : GridDebugObject
{
    [SerializeField] private TextMeshPro _gCostText;
    [SerializeField] private TextMeshPro _hCostText;
    [SerializeField] private TextMeshPro _fCostText;
    [SerializeField] private SpriteRenderer _isWalkableVisual;

    private PathNode _pathNote;

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        _pathNote = gridObject as PathNode;
    }

    protected override void UpdateVisualData()
    {
        base.UpdateVisualData();

        _gCostText.SetText(_pathNote.GetGCost().ToString());
        _hCostText.SetText(_pathNote.GetHCost().ToString());
        _fCostText.SetText(_pathNote.GetFCost().ToString());

        Color newColor = new Color();
        newColor = _pathNote.IsWalkable() ? Color.green : Color.red;
        newColor.a = 0.2f;

        _isWalkableVisual.color = newColor;
    }

}
