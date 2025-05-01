using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private Transform _bulletTrail;
    [SerializeField] private ParticleSystem _bulletHitVFX;
    private Vector3 _targetPosition;

    private void Update()
    {
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;

        float moveSpeed = 200f;
        float distanceBeforeMoveToTargetUnit = Vector3.Distance(transform.position, _targetPosition);

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        float distanceAfterMoveToTargetUnit = Vector3.Distance(transform.position, _targetPosition);
        if (!(distanceBeforeMoveToTargetUnit < distanceAfterMoveToTargetUnit)) return;

        transform.position = _targetPosition;
        _bulletTrail.parent = null;
        Destroy(gameObject);

        Instantiate(_bulletHitVFX, transform.position, Quaternion.identity);
    }

    public void Setup(Vector3 targetShootAtPosition)
    {
        _targetPosition = targetShootAtPosition;
    }
}
