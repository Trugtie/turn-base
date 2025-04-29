using UnityEngine;

public class Unit : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private Animator _animator;

    private Vector3 _targetPosition;

    private GridPosition _gridPosition;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _targetPosition = transform.position;
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
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

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition)
        {
            LevelGrid.Instance.MoveUnitToGridPosition(this, _gridPosition, newGridPosition);
            _gridPosition = newGridPosition;
        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public override string ToString()
    {
        return $"Unit: {gameObject.name}";
    }
}
