using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    private const string IS_OPEN = "isOpen";

    [SerializeField] private bool _isOpen;

    private Animator _animator;
    private GridPosition _gridPosition;

    private Action _onActionComplete;
    private bool _isActive;
    private float _timer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_isActive) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _isActive = false;
            _onActionComplete();
        }
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetDoorAtGridPosition(_gridPosition, this);

        if (_isOpen)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    public void Interact(Action onActionComplete)
    {
        _isActive = true;
        _timer = 0.5f;
        _onActionComplete = onActionComplete;

        if (_isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private void Open()
    {
        _isOpen = true;
        PathFinding.Instance.SetWalkableGridPosition(_gridPosition, true);
        _animator.SetBool(IS_OPEN, _isOpen);
    }

    private void Close()
    {
        _isOpen = false;
        PathFinding.Instance.SetWalkableGridPosition(_gridPosition, false);
        _animator.SetBool(IS_OPEN, _isOpen);
    }
}
