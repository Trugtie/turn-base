using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour

{
    [SerializeField] private Transform _rootBone;
    [SerializeField] private Transform _ragollPrefab;
    private HealthSystem _healthSystem;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        _healthSystem.OnDead += _healthSystem_OnDead;
    }

    private void _healthSystem_OnDead(object sender, System.EventArgs e)
    {
        Transform ragdollInstance = Instantiate(_ragollPrefab, transform.position, Quaternion.identity);
        ragdollInstance.GetComponent<UnitRagdoll>().Setup(_rootBone, transform);
    }
}
