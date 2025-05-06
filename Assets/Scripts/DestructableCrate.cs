using System;
using UnityEngine;

public class DestructableCrate : MonoBehaviour
{
    public static event EventHandler OnAnyDestructableCrateDamge;
    private GridPosition _gridPosition;

    private void Awake()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetGridPosition() => _gridPosition;

    public void Damge()
    {
        Destroy(gameObject);
        OnAnyDestructableCrateDamge?.Invoke(this, EventArgs.Empty);
    }
}
