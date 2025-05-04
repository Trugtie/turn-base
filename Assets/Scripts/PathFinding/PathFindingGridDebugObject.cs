using TMPro;
using UnityEngine;

public class PathFindingGridDebugObject : GridDebugObject
{
    [SerializeField] private TextMeshProUGUI _gCostText;
    [SerializeField] private TextMeshProUGUI _hCostText;
    [SerializeField] private TextMeshProUGUI _fCostText;

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
    }

}
