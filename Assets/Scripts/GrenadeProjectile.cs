using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField] private ParticleSystem _grenadeVFX;
    [SerializeField] private TrailRenderer _grenadeTrail;
    [SerializeField] private AnimationCurve _moveArcYCurve;

    private Action _onGrenadeExplodedCallback;
    private Vector3 _targetPosition;
    private Vector3 _positionXZ;
    private float _totalDistance;

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = (_targetPosition - _positionXZ).normalized;
        float toTargetDistance = Vector3.Distance(_positionXZ, _targetPosition);
        float moveSpeed = 15f;
        _positionXZ += moveDirection * moveSpeed * Time.deltaTime;

        float distanceNormalize = 1 - toTargetDistance / _totalDistance;
        float maxHeightArc = _totalDistance / 4f;
        float positionY = _moveArcYCurve.Evaluate(distanceNormalize) * maxHeightArc;

        transform.position = new Vector3(_positionXZ.x, positionY, _positionXZ.z);

        float reachedTargetDistance = 0.2f;
        if (toTargetDistance <= reachedTargetDistance)
        {
            HandleExplosion();
            _grenadeTrail.transform.parent = null;
            Destroy(gameObject);
            Instantiate(_grenadeVFX, _targetPosition + Vector3.up * 1f, Quaternion.identity);
            _onGrenadeExplodedCallback();
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
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
        _totalDistance = Vector3.Distance(transform.position, _targetPosition);
        _positionXZ = transform.position;
        _positionXZ.y = 0;
        _onGrenadeExplodedCallback = onGrenadeBehaviourComplete;
    }
}
