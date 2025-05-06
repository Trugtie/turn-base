using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    private Action _onGrenadeExplodedCallback;
    private Vector3 _targetPosition;

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        float reachedTargetDistance = 0.2f;
        float toTargetDistance = Vector3.Distance(transform.position, _targetPosition);

        float moveSpeed = 15f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (toTargetDistance <= reachedTargetDistance)
        {
            HandleExplosion();
            Destroy(gameObject);
            _onGrenadeExplodedCallback();
            return;
        };
    }

    private void HandleExplosion()
    {
        float explosionRadius = 4f;
        int explosionDamge = 30;
        Collider[] damgeableColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider damgeableCollider in damgeableColliders)
        {
            if (!damgeableCollider.TryGetComponent<Unit>(out Unit unit)) continue;

            unit.TakeDamge(explosionDamge);
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        _onGrenadeExplodedCallback = onGrenadeBehaviourComplete;
    }
}
