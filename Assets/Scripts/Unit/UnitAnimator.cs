using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string SHOOT = "Shoot";
    private const string SWORD_SLASH = "SwordSlash";

    [SerializeField] private Transform _bulletProjectilePrefab;
    [SerializeField] private Transform _shootPositionTransform;
    [SerializeField] private Transform _rifleTransfrom;
    [SerializeField] private Transform _swordTransform;


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

        if (TryGetComponent(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStart += SwordAction_OnSwordActionStart;
            swordAction.OnSwordActionEnd += SwordAction_OnSwordActionEnd;
        }

        EquipRifle();
    }

    private void SwordAction_OnSwordActionEnd(object sender, System.EventArgs e)
    {
        EquipRifle();
    }

    private void SwordAction_OnSwordActionStart(object sender, System.EventArgs e)
    {
        EquipSword();
        _animator.SetTrigger(SWORD_SLASH);
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

    private void EquipSword()
    {
        _rifleTransfrom.gameObject.SetActive(false);
        _swordTransform.gameObject.SetActive(true);
    }

    private void EquipRifle()
    {
        _rifleTransfrom.gameObject.SetActive(true);
        _swordTransform.gameObject.SetActive(false);
    }
}
