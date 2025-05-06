using UnityEngine;

public class PathFindingUpdater : MonoBehaviour
{
    private void Start()
    {
        DestructableCrate.OnAnyDestructableCrateDamge += DestructableCrate_OnAnyDestructableCrateDamge;
    }

    private void DestructableCrate_OnAnyDestructableCrateDamge(object sender, System.EventArgs e)
    {
        DestructableCrate destructableCrate = sender as DestructableCrate;
        PathFinding.Instance.SetWalkableGridPosition(destructableCrate.GetGridPosition(), true);
    }
}
