using UnityEngine;

public class Unit : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private Animator _animator;

    private Vector3 _targetPosition;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _targetPosition = transform.position;
    }

    private void Update()
    {
        float stopThresoldDistance = 0.1f;
        float moveDistance = Vector3.Distance(transform.position, _targetPosition);
        _animator.SetBool(IS_WALKING, false);
        if (moveDistance < stopThresoldDistance) return;

        float speed = 4f;
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        moveDirection.y = 0;
        transform.position += speed * Time.deltaTime * moveDirection;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        _animator.SetBool(IS_WALKING, true);
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
