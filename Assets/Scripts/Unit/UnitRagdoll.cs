using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform _ragdollRootBone;
    [SerializeField] private float _explosionRange = 1f;
    [SerializeField] private float _explosionForce = 1000f;

    public void Setup(Transform originalBone, Transform originalTransform)
    {
        transform.position = originalTransform.position;
        transform.rotation = originalTransform.rotation;
        MatchAllChildTransform(originalBone, _ragdollRootBone);

        Vector3 randomDirection = new Vector3(Random.Range(-1f, +1f), 0, Random.Range(-1f, +1f));
        Vector3 explosionPosition = transform.position + randomDirection;

        ApplyExplosionToRagdoll(_ragdollRootBone, _explosionForce, explosionPosition, _explosionRange);
    }

    private void MatchAllChildTransform(Transform originalRootBone, Transform cloneBone)
    {
        foreach (Transform child in originalRootBone)
        {
            Transform cloneChild = cloneBone.Find(child.name);

            if (cloneChild == null) return;

            cloneChild.position = child.position;
            cloneChild.rotation = child.rotation;

            MatchAllChildTransform(child, cloneChild);
        }
    }

    private void ApplyExplosionToRagdoll(Transform rootBone, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in rootBone)
        {
            if (!child.TryGetComponent(out Rigidbody childRigidbody)) continue;
            childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
