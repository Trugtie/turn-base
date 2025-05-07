using System;
using UnityEngine;

public class DestructableCrate : MonoBehaviour
{
    [SerializeField] private Transform _createDestroyedPrefab;
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
        Transform createDestroyedInstance = Instantiate(_createDestroyedPrefab, transform.position, Quaternion.identity);
        ApplyExplosionToCrateDestoyed(createDestroyedInstance, 150f, transform.position, 10f);
        OnAnyDestructableCrateDamge?.Invoke(this, EventArgs.Empty);
    }

    private void ApplyExplosionToCrateDestoyed(Transform rootBone, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in rootBone)
        {
            if (!child.TryGetComponent(out Rigidbody childRigidbody)) continue;
            childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            ApplyExplosionToCrateDestoyed(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
