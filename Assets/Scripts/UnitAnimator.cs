using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";

    private const string SHOOT = "Shoot";

    [SerializeField] private Transform _bulletProjectilePrefab;
    [SerializeField] private Transform _shootPositionTransform;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (TryGetComponent(out MoveAction moveAction))
        {
            moveAction.OnMoveStart += MoveAction_OnMoveStart;
            moveAction.OnMoveEnd += MoveAction_OnMoveEnd;
        }

        if (TryGetComponent(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }

    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        _animator.SetTrigger(SHOOT);

        Transform bulletTransform = Instantiate(_bulletProjectilePrefab, _shootPositionTransform.position, Quaternion.identity);
        Vector3 targetUnitShootAtPostion = e.TargetUnit.GetWorldPosition();
        targetUnitShootAtPostion.y = _shootPositionTransform.position.y;
        bulletTransform.GetComponent<BulletProjectile>().Setup(targetUnitShootAtPostion);
    }

    private void MoveAction_OnMoveEnd(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_WALKING, false);
    }

    private void MoveAction_OnMoveStart(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_WALKING, true);
    }
}
